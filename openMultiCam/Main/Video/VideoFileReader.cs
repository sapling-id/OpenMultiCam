using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace openMultiCam.Utils {
    public class VideoFileReader {
        private String filePath;
        private VideoFrameDeserializer videoFrameDeserializer;
        public VideoFileMetaData videoFileMetaData { get; private set; }


        public VideoFileReader(String filePath) {
            this.filePath = filePath;
            videoFileMetaData = new VideoFileMetaData(filePath);
            videoFrameDeserializer = new VideoFrameDeserializer(videoFileMetaData);
        }

        public void reset() {
            videoFrameDeserializer.reset();
        }

        public void dispose() {
            videoFrameDeserializer.dispose();
        }

        public Bitmap getNextFrame() {
            return videoFrameDeserializer.getNextFrame();
        }

        public Bitmap getFrameAtIndex(int index) {
            return VideoFrameDeserializer.loadFrameAtIndex(filePath, index);
        }
    }
}
