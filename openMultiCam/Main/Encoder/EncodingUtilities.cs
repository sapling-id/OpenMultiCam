using openMultiCam.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace openMultiCam.Main.Encoder {
    public class EncodingUtilities {

        public delegate void ProgressUpdate(float progress, bool finished, double eta);
        public ProgressUpdate update { get; set; }

        private VideoFileReader videoFileReader;
        private Thread encodingThread;
        private string filePath;
        private int quality;


        public EncodingUtilities(String filePath, int quality) {
            this.videoFileReader = new VideoFileReader(filePath);
            this.quality = quality;
            this.filePath = filePath;
        }

        private void updateData(float progress, bool finished, double eta) {

            if (update != null) {
                update(progress, finished, eta);
            }
        }

        private void encodeGif() {
            Bitmap currentFrame;
            GifEncoder gifEncoder = new GifEncoder((int)videoFileReader.videoFileMetaData.targetFramerate, videoFileReader.videoFileMetaData.filePath + "\\" + CamConstants.ENCODED_FILE_NAME_GIF, quality, videoFileReader.videoFileMetaData.frameCount);
            gifEncoder.update += updateData;

            while (true) {
                currentFrame = videoFileReader.getNextFrame();

                if (currentFrame != null) {
                    gifEncoder.writeToFrameBuffer(currentFrame);
                } else {
                    break;
                }
            }

            gifEncoder.lastFrame = true;
            while(!gifEncoder.finished) {
                Thread.Sleep(100);
            }
            updateData(1f, true, 0);
            Process.Start(filePath);
        }

        public void encodeAsGif() {
            encodingThread = new Thread(encodeGif);
            encodingThread.Start();
        }

    }
}
