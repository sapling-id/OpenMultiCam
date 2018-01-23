using Gif.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace openMultiCam.Utils {
    public class GifEncoder {
        public bool lastFrame { get; set; }
        public bool finished { get; set; }

        private float encodingProgress { get; set; }
        private bool encodingFinished { get; set; }
        private double estimatedTimeOfArrival { get; set; }

        public delegate void ProgressUpdate(float progress, bool finished, double eta);
        public ProgressUpdate update { get; set; }

        private AnimatedGifEncoder encoder;
        private Queue<Bitmap> imageQueue;
        private Thread writingThread;
        private int encodedFrameCount;
        private int totalFrameCount;
        private string destinationFilePath;
        private int frameRate;

        public GifEncoder(int frameRate, String destinationFilePath, int quality, int totalFrameCount) {
            imageQueue = new Queue<Bitmap>();
            lastFrame = false;
            finished = false;
            encodedFrameCount = 0;
            this.frameRate = frameRate;
            this.destinationFilePath = destinationFilePath;
            this.totalFrameCount = totalFrameCount;


            
            encoder = new AnimatedGifEncoder();
            encoder.SetFrameRate(frameRate);
            encoder.SetQuality(quality);
            encoder.SetRepeat(0);
            encoder.Start(destinationFilePath);

            writingThread = new Thread(encode);
            writingThread.Start();
        }

        public void writeToFrameBuffer(Bitmap frameToWrite) {
            imageQueue.Enqueue(frameToWrite);
        }

        private void encode() {
                Stopwatch stopwatch = new Stopwatch();
                updateData((float)encodedFrameCount / totalFrameCount,
                false,
                stopwatch.Elapsed.TotalSeconds * (totalFrameCount - encodedFrameCount));


                while (true) {
                    if (imageQueue.Count > 0) {
                        stopwatch.Restart();

                        encoder.AddFrame(imageQueue.Dequeue());
                        encodedFrameCount++;
                        stopwatch.Stop();
                        updateData((float)encodedFrameCount / totalFrameCount,
                                    false,
                                    stopwatch.Elapsed.TotalSeconds * (totalFrameCount - encodedFrameCount));
                    }

                    if (lastFrame && imageQueue.Count == 0) {
                        finalizeEncoding();
                        finished = true;
                        break;
                    }
                }
        }


        private void updateData(float progress, bool finished, double eta) {

            if (update != null) {
                update(progress, finished, eta);
            }
        }

        private void finalizeEncoding() {
            encoder.Finish();
        }
    }
}
