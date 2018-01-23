using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openMultiCam.Utils {
    public interface RecordingRegion {

        void getWidth(ScreenCaptureUtilities screenCaptureUtils);
        void getHeight(ScreenCaptureUtilities screenCaptureUtils);
        void getOffestX(ScreenCaptureUtilities screenCaptureUtils);
        void getOffsetY(ScreenCaptureUtilities screenCaptureUtils);
    }
}
