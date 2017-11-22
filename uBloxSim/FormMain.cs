using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace uBloxSim
{
    struct STargetPoint
    {
        public double Latitude;
        public double Longitude;
    };

    public partial class FormMain : Form
    {
        // Targets
        STargetPoint TargetPoint;

        // Map
        Image mapBitmap;
        private int CircleSize = 0;

        // Double buffering
        BufferedGraphicsContext DBcurrentContext;
        BufferedGraphics DBmyBuffer;
        BufferedGraphics DBmyBufferSmall;

        // PositionUpdater
        double DeltaLongitude = 0; // degrees per second
        double DeltaLatitude = 0; // degrees per second
        PointF DeltaDirection = new Point(1, 0);

        public FormMain()
        {
            InitializeComponent();

            // Create DB objects
            DBcurrentContext = BufferedGraphicsManager.Current;
            DBmyBuffer = DBcurrentContext.Allocate(pictureBoxMap.CreateGraphics(), pictureBoxMap.DisplayRectangle);
            DBmyBufferSmall = DBcurrentContext.Allocate(pictureBoxDirection.CreateGraphics(), pictureBoxDirection.DisplayRectangle);

            // load amp
            mapBitmap = Bitmap.FromFile("map.bmp");

            comboBoxSpeed.SelectedIndex = 0;

            // enumerate serial ports
            string[] ports = SerialPort.GetPortNames();
            comboBoxPorts.Items.AddRange(ports);
            if (ports.Length > 0) comboBoxPorts.SelectedIndex = ports.Length - 1;
        }

        // Main Timer
        private void timer20Hz_Tick(object sender, EventArgs e)
        {
            // Update Map and Text fields
            Update20Hz();

            // Send uBlox Messages
            SendUBloxStuff();
        }

        private void SendUBloxStuff()
        {
            if( serialPortSim.IsOpen)
            {
                // port open, check messages
                if( checkBoxSendHNRPVT.Checked)
                {
                    // send HNRPVT Message
                    SendUBLOXHNRPVTMsg(serialPortSim, TargetPoint);
                }
            }
        }

        private void SendUBLOXHNRPVTMsg(SerialPort serialPrt, STargetPoint targetPoint)
        {
            byte[] dataToSend = GenerateHNRPVTMSG(targetPoint);
            serialPrt.Write(dataToSend, 0, dataToSend.Length);
        }


        // Custom TX Messages
        byte[] GenerateHNRPVTMSG(STargetPoint targetPoint)
        {
            byte[] localBuffer = new byte[72];

            // add data type to data            
            uint gpsTimeMS = (uint)DateTime.Now.TimeOfDay.TotalMilliseconds; // miliseconds from start of day, SHOULD BE START OF WEEK!)
            byte[] gpsTimeArr = BitConverter.GetBytes(gpsTimeMS);
            byte[] yearUTCArr = BitConverter.GetBytes((UInt16)2017);

            byte[] longitudeArr = BitConverter.GetBytes((int)(targetPoint.Longitude * 1e7));
            byte[] latitudeArr  = BitConverter.GetBytes((int)(targetPoint.Latitude * 1e7));

            byte[] heightArr = BitConverter.GetBytes((int)120 * 1000);
            byte[] heightMSLArr = BitConverter.GetBytes((int)150 * 1000);

            byte[] speedArr = BitConverter.GetBytes((int)30 * 1000);

            double heading = Math.Atan2(DeltaDirection.X, DeltaDirection.Y) * 180/Math.PI;
            if (heading < 0) heading += 360;
            byte[] headingArr = BitConverter.GetBytes((int)(heading*1e5));

            byte[] accArr = BitConverter.GetBytes((uint)(950)); // mm
            byte[] accHdgArr = BitConverter.GetBytes((uint)(1.5*1e5)); // mm

            gpsTimeArr.CopyTo(localBuffer, 0); // gps time
            yearUTCArr.CopyTo(localBuffer, 4); // year
            localBuffer[6] = (byte)DateTime.Now.Month; // month
            localBuffer[7] = (byte)DateTime.Now.Day; // day
            localBuffer[8] = (byte)DateTime.Now.Hour; // hour
            localBuffer[9] = (byte)DateTime.Now.Minute; // minute
            localBuffer[10] = (byte)DateTime.Now.Second; // second
            localBuffer[11] = 0x07; // Valid flags
            localBuffer[16] = 0x04; // GPS + DR
            localBuffer[17] = 0x1D; // GPSFIxOK, WKNSET, TOWSET, headingValid
            localBuffer[18] = 0x00; // Reserved
            localBuffer[19] = 0x00; // Reserved
            longitudeArr.CopyTo(localBuffer, 20);
            latitudeArr.CopyTo(localBuffer, 24);

            heightArr.CopyTo(localBuffer, 28);
            heightMSLArr.CopyTo(localBuffer, 32);

            speedArr.CopyTo(localBuffer, 36); // ground speed
            speedArr.CopyTo(localBuffer, 40); // 3D Speed

            headingArr.CopyTo(localBuffer, 44); // Heading of motion
            headingArr.CopyTo(localBuffer, 48); // Heading of vehicle

            accArr.CopyTo(localBuffer, 52); // horizontal accuracy
            accArr.CopyTo(localBuffer, 56); // vertical accuracy
            accArr.CopyTo(localBuffer, 60); // speed accuracy

            accHdgArr.CopyTo(localBuffer, 64); // heading acc
            
            return GenerateUbloxTXPacket(0x28, 0x00, localBuffer, 72);
        }

        byte[] GenerateESFMeasMSG(uint timetag, byte dataTypeID, uint providerData)
        {
            byte[] localBuffer = new byte[12];

            // add data type to data
            providerData = (providerData & 0x00FFFFFF) + (uint)(dataTypeID << 24);

            byte[] timeTagArr = BitConverter.GetBytes(timetag);
            byte[] providerDataArr = BitConverter.GetBytes(providerData);

            timeTagArr.CopyTo(localBuffer, 0);
            localBuffer[4] = 0; // flags = 0
            localBuffer[5] = 0; // flags = 0
            localBuffer[6] = 0; // Data Provider = 0
            localBuffer[7] = 0; // Data Provider = 0

            providerDataArr.CopyTo(localBuffer, 8); // data (speed)

            return GenerateUbloxTXPacket(0x10, 0x02, localBuffer, 12);
        }

        // uBlox Generator
        private byte[] GenerateUbloxTXPacket(byte classID, byte id, byte[] data, int length)
        {
            byte[] packet = new byte[length + 8];
            // assemble header
            packet[0] = 0xB5; // SYNC1
            packet[1] = 0x62; // SYNC2
            packet[2] = classID; // CLASS
            packet[3] = id; // ID
            packet[4] = (byte)(length % 256);
            packet[5] = (byte)(length / 256);

            // copy payload
            data.CopyTo(packet, 6);

            // add checksum
            CHKSUM chk = CalculateCheckSum(packet, length);
            packet[length + 6] = chk.CK_A;
            packet[length + 7] = chk.CK_B;

            return packet; // return total packet, length: (2xSYNC + CLASS + ID + 2xLEN + 2xCHKSUM + LENGTH) (data+8)
        }

        private struct CHKSUM
        {
            public byte CK_A;
            public byte CK_B;
        };

        // packet is whole packet (starts with SYNC1), chk "data" is 4 bytes longer
        CHKSUM CalculateCheckSum(byte[] packet, int length)
        {
            byte ck_A = 0;
            byte ck_B = 0;

            for (int i = 2; i != length + 4 + 2; i++)
            {
                ck_A = (byte)(ck_A + packet[i]);
                ck_B = (byte)(ck_B + ck_A);
            }

            CHKSUM chk;
            chk.CK_A = ck_A;
            chk.CK_B = ck_B;

            return chk;
        }

      

        // Open Comm Port
        private void buttonOpen_Click(object sender, EventArgs e)
        {
            try
            {
                serialPortSim.BaudRate = 115200;
                serialPortSim.PortName = (string)comboBoxPorts.SelectedItem;
                serialPortSim.Open();
                buttonOpen.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void pictureBoxMap_Click(object sender, EventArgs e)
        {
            Point point = pictureBoxMap.PointToClient(Cursor.Position);
            OnMouseClick(point);
        }

        public void OnMouseClick(Point point)
        {
            float DPI = 1.0F; // TODO!!!
            float mapX = point.X * DPI;
            float mapY = point.Y * DPI;

            double lon, lat;
            ConvertLocationPix2LL(out lon, out lat, mapX, mapY);
            TargetPoint.Latitude = lat;
            TargetPoint.Longitude = lon;
        }

        private void pictureBoxDirection_Click(object sender, EventArgs e)
        {
            Point point = pictureBoxDirection.PointToClient(Cursor.Position);

            DeltaDirection.X = (point.X - pictureBoxDirection.Size.Width / 2);
            DeltaDirection.Y = -(point.Y - pictureBoxDirection.Size.Height / 2);

            // normalize
            float length = (float)Math.Sqrt(DeltaDirection.X * DeltaDirection.X + DeltaDirection.Y * DeltaDirection.Y);
            DeltaDirection.X = DeltaDirection.X / length;
            DeltaDirection.Y = DeltaDirection.Y / length;

            comboBoxSpeed_SelectedIndexChanged(this, new EventArgs());
        }

        public void Update20Hz()
        {
            DrawMap();

            // update text
            textBoxLatitude.Text = string.Format("{0:0.00000000}", TargetPoint.Latitude);
            textBoxLongitude.Text = string.Format("{0:0.00000000}", TargetPoint.Longitude);
            textBoxLatitudeMovement.Text = string.Format("{0} deg/s", DeltaLatitude.ToString("0.000000"));
            textBoxLongitudeMovement.Text = string.Format("{0} deg/s", DeltaLongitude.ToString("0.000000"));

            // Update position
            UpdateTargetPosition();            
        }

        private void comboBoxSpeed_SelectedIndexChanged(object sender, EventArgs e)
        {
            double degToMeters = 110300; // approx
            // recalculate speed
            double speedDPS = 0;
            if (comboBoxSpeed.SelectedIndex == 0) speedDPS = 0;
            if (comboBoxSpeed.SelectedIndex == 1) speedDPS = 10/ degToMeters;
            if (comboBoxSpeed.SelectedIndex == 2) speedDPS = 100 / degToMeters;
            if (comboBoxSpeed.SelectedIndex == 3) speedDPS = 1000 / degToMeters;
            if (comboBoxSpeed.SelectedIndex == 4) speedDPS = 10000 / degToMeters;
            if (comboBoxSpeed.SelectedIndex == 5) speedDPS = 100000 / degToMeters;
            if (comboBoxSpeed.SelectedIndex == 6) speedDPS = 3000000 / degToMeters;

            DeltaLongitude = DeltaDirection.X * speedDPS;
            DeltaLatitude = DeltaDirection.Y * speedDPS;
        }

        public void UpdateTargetPosition()
        {
            TargetPoint.Latitude += DeltaLatitude/20; // divide by rate
            if (TargetPoint.Latitude > 90) TargetPoint.Latitude -= 180;
            if (TargetPoint.Latitude < -90) TargetPoint.Latitude += 180;

            TargetPoint.Longitude += DeltaLongitude / 20; // divide by rate
            if (TargetPoint.Longitude > 180) TargetPoint.Longitude -= 360;
            if (TargetPoint.Longitude < -180) TargetPoint.Longitude += 360;
        }

        public void DrawMap()
        {
            // clear backbuffer
            DBmyBuffer.Graphics.Clear(Color.Black);

            if (mapBitmap == null) return;

            // draw map
            DBmyBuffer.Graphics.DrawImageUnscaled(mapBitmap, pictureBoxMap.DisplayRectangle);

            // draw location
            float mapX, mapY;
            ConvertLocationLL2Pix(TargetPoint.Longitude, TargetPoint.Latitude, out mapX, out mapY);
            // clip to map!
            if (mapX < 0) mapX = 0;
            if (mapY < 0) mapY = 0;
            if (mapX > MapImageSizePix.Width) mapX = (float)MapImageSizePix.Width;
            if (mapY > MapImageSizePix.Height) mapY = (float)MapImageSizePix.Height;
            DBmyBuffer.Graphics.FillEllipse(new SolidBrush(Color.White), mapX - 5, mapY - 5, 10, 10);
            DBmyBuffer.Graphics.DrawEllipse(new Pen(Color.Red, 3), mapX - 5 - CircleSize / 2, mapY - 5 - CircleSize / 2, 10 + CircleSize, 10 + CircleSize);
            if (++CircleSize > 30) CircleSize = 0;           
            
            // render to screen            
            DBmyBuffer.Render();

            // Draw Delta Direction
            // clear backbuffer
            DBmyBufferSmall.Graphics.Clear(SystemColors.ControlDark);

            PointF centerP = new PointF(pictureBoxDirection.Size.Width/2, pictureBoxDirection.Size.Height / 2);
            PointF directionP = new PointF(DeltaDirection.X * 50 + pictureBoxDirection.Size.Width / 2, -DeltaDirection.Y * 50 + pictureBoxDirection.Size.Height / 2);

            DBmyBufferSmall.Graphics.DrawLine(new Pen(Color.LightGoldenrodYellow, 5), centerP, directionP);

            // render to screen            
            DBmyBufferSmall.Render();
        }



        // Helpers
        private double CenterLatitude = 0;
        private double CenterLongitude = 0;
        int ZoomLevel = 2;
        private Size MapImageSizePix = new Size(1200, 930);
        public void ConvertLocationLL2Pix(double longitude, double latitude, out float mapX, out float mapY)
        {
            double deltaX = longitude - CenterLongitude;
            double pixelX = (deltaX / 360) * 300 * Math.Pow(2, ZoomLevel);
            mapX = (float)(pixelX + MapImageSizePix.Width / 2);

            double sinLatitudeCenter = Math.Sin(CenterLatitude * Math.PI / 180);
            double pixelYCenter = (0.5 - Math.Log((1 + sinLatitudeCenter) / (1 - sinLatitudeCenter)) / (4 * Math.PI)) * 300 * Math.Pow(2, ZoomLevel); // center pix
            double sinLatitude = Math.Sin(latitude * Math.PI / 180);
            double pixelY = (0.5 - Math.Log((1 + sinLatitude) / (1 - sinLatitude)) / (4 * Math.PI)) * 300 * Math.Pow(2, ZoomLevel);
            mapY = (float)(pixelY - pixelYCenter + MapImageSizePix.Height / 2);
        }

        public void ConvertLocationPix2LL(out double longitude, out double latitude, float mapX, float mapY)
        {
            double pixelX = mapX - MapImageSizePix.Width / 2;
            double deltaX = pixelX * 360 / (300 * Math.Pow(2, ZoomLevel));
            longitude = deltaX + CenterLongitude;

            double sinLatitudeCenter = Math.Sin(CenterLatitude * Math.PI / 180);
            double pixelYCenter = (0.5 - Math.Log((1 + sinLatitudeCenter) / (1 - sinLatitudeCenter)) / (4 * Math.PI)) * 300 * Math.Pow(2, ZoomLevel); // center pix
            double pixelY = mapY + pixelYCenter - MapImageSizePix.Height / 2;
            double deltaY = 0.5 - pixelY / (300 * Math.Pow(2, ZoomLevel));
            latitude = 90 - 360 * Math.Atan(Math.Exp(-deltaY * 2 * Math.PI)) / Math.PI;
        }        
    }
}
