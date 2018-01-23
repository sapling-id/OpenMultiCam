using Gif.Components;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace openMultiCam.Utils {
    public class GifEncoder {
        private AnimatedGifEncoder encoder;

        public GifEncoder(int frameRate, String destinationFilePath) {
            encoder = new AnimatedGifEncoder();
            encoder.SetFrameRate(frameRate);
            encoder.SetQuality(100);
            encoder.SetRepeat(0);
            encoder.Start(destinationFilePath);
        }

        public void writeFrame(Bitmap currentFrame) {
            encoder.AddFrame(currentFrame);
        }

        public void finalizeEncoding() {
            encoder.Finish();
        }
    }
}
