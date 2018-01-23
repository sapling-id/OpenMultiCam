using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openMultiCam.Main.Encoder {
    public interface Encoder {
        bool lastFrame { get; set; }
        bool finished { get; set; }

        void writeToFrameBuffer(Bitmap frameToWrite);
        void writeFrame(Bitmap frameToWrite);
        void finalize();
    }
}
