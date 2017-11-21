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

        // TODO!!!
        private void SendUBLOXHNRPVTMsg(SerialPort serialPrt, STargetPoint targetPoint)
        {
            byte[] buffer = new byte[100];
            serialPrt.Write(buffer, 0, buffer.Length);
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
