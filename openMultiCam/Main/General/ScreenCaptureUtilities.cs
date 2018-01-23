using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using openMultiCam.Utils;
using System.Drawing.Imaging;

namespace openMultiCam {
    public interface ScreenCaptureUtilities {
        int width {
            get;
            set;
        }
        int height {
            get;
            set;
        }
        int xOffset {
            get;
            set;
        }
        int yOffset {
            get;
            set;
        }

        Bitmap captureScreen();
    }
}
