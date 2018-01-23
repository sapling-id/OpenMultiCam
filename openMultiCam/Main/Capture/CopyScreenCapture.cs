using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;
using openMultiCam.Utils;
using System.Drawing.Imaging;

namespace openMultiCam {
    public class CopyScreenCapture : ScreenCaptureUtilities {
        private Bitmap capturedScreenRegion;
        private Graphics captureGraphicsContext;
        private int _width;
        public int width {
            get { return _width; }
            set {
                if (value % 2 != 0) {
                    _width = value-1;
                } else {
                    _width = value;
                }
                buildGraphicsContext();
            }
        }
        private int _height;
        public int height {
            get { return _height; }
            set {
                if(value % 2 != 0) {
                    _height = value-1;
                } else {
                    _height = value;
                }

                buildGraphicsContext();
            }
        }
        public int xOffset { get; set; }
        public int yOffset { get; set; }

        public CopyScreenCapture() {
            xOffset = 0;
            yOffset = 0;
            //_width = Screen.PrimaryScreen.Bounds.Width;
            //_height = Screen.PrimaryScreen.Bounds.Height;
            _width = (int)System.Windows.SystemParameters.PrimaryScreenWidth;
            _height = (int)System.Windows.SystemParameters.PrimaryScreenHeight;
            capturedScreenRegion = new Bitmap(_width, _height);
        }

        public void debugCaptureToFile() {
            captureGraphicsContext.CopyFromScreen(xOffset, yOffset, 0, 0, new Size(_width, _height));
            capturedScreenRegion.Save(CamConstants.WORKSPACE_PATH + "debugFile.png", ImageFormat.Png);
        }

        public Bitmap captureScreen() {
            captureGraphicsContext.CopyFromScreen(xOffset, yOffset, 0, 0, new Size(_width, _height));
            return capturedScreenRegion;
        }

        private void buildGraphicsContext() {
            capturedScreenRegion = new Bitmap(_width, _height);
            captureGraphicsContext = Graphics.FromImage(capturedScreenRegion);
        }
    }
}
