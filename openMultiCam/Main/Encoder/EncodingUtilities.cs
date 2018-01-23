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
        public float encodingProgress { get
            {
                return _encodingProgress;
            }
                private set { } }
        public bool encodingFinished {
            get
            {
                return _encodingFinished;
            }
            private set { }
        }
        public double estimatedTimeOfArrival
        {
            get
            {
                return _estimatedTimeOfArrival;
            }
            private set { }
        }

        private float _encodingProgress;
        private bool _encodingFinished;
        private double _estimatedTimeOfArrival;
        public delegate void ProgressUpdate();
        public ProgressUpdate update { get; set; }

        private VideoFileReader videoFileReader;
        private Thread encodingThread;
        private string filePath;


        public EncodingUtilities(String filePath) {
            this.videoFileReader = new VideoFileReader(filePath);
            this.filePath = filePath;
            encodingProgress = 0;
            encodingFinished = false;
        }

        private void updateData(float progress, bool finished, double eta) {
            _encodingProgress = progress;
            _encodingFinished = finished;
            _estimatedTimeOfArrival = eta;


            if (update != null) {
                update();
            }
        }

        private void encodeGif() {
            Bitmap currentFrame;

            Stopwatch stopwatch = new Stopwatch();
            int encodedFrameCount = 0;
            GifEncoder gifEncoder = new GifEncoder((int)videoFileReader.videoFileMetaData.targetFramerate, videoFileReader.videoFileMetaData.filePath + "\\" + CamConstants.ENCODED_FILE_NAME_GIF);
            while (true) {
                currentFrame = videoFileReader.getNextFrame();

                if (currentFrame != null) {
                    stopwatch.Restart();
                    gifEncoder.writeFrame(currentFrame);
                    stopwatch.Stop();

                    encodedFrameCount++;
                    updateData((float)encodedFrameCount / videoFileReader.videoFileMetaData.frameCount,
                                false,
                                stopwatch.Elapsed.TotalSeconds * (videoFileReader.videoFileMetaData.frameCount - encodedFrameCount));
                } else {
                    break;
                }
            }
            gifEncoder.finalizeEncoding();
            updateData(1f, true, 0);
            Process.Start(filePath);
        }

        public void encodeAsGif() {
            encodingThread = new Thread(encodeGif);
            encodingThread.Start();
        }

    }
}
