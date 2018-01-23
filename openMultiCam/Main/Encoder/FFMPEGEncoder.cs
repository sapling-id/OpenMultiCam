using Accord.Video.FFMPEG;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace openMultiCam.Main.Encoder  {
    public class FFMPEGEncoder : Main.Encoder.Encoder {
        public bool finished { get; set; }
        public bool lastFrame { get; set; }

        public delegate void ProgressUpdate(float progress, bool finished, double eta);
        public ProgressUpdate update { get; set; }

        private Queue<Bitmap> imageQueue;
        private VideoFileWriter encoder;
        private Thread writingThread;
        private int encodedFrameCount;
        private int totalFrameCount;
        private string destinationFilePath;
        private int frameRate;


        public FFMPEGEncoder(int frameRate, String destinationFilePath, int quality, int totalFrameCount, int width, int height, bool threaded) {
            imageQueue = new Queue<Bitmap>();
            lastFrame = false;
            finished = false;
            encodedFrameCount = 0;
            this.frameRate = frameRate;
            this.destinationFilePath = destinationFilePath;
            this.totalFrameCount = totalFrameCount;

            encoder = new VideoFileWriter();
            encoder.FrameRate = frameRate;
            encoder.Width = width;
            encoder.Height = height;
            encoder.VideoCodec = VideoCodec.Vp9;

            encoder.Open(destinationFilePath);

            if(threaded) {
                writingThread = new Thread(encode);
                writingThread.Start();
            }

        }


        private void encode() {
            Stopwatch stopwatch = new Stopwatch();
            updateData((float)encodedFrameCount / totalFrameCount,
            false,
            stopwatch.Elapsed.TotalSeconds * (totalFrameCount - encodedFrameCount));


            while (true) {
                if (imageQueue.Count > 0) {
                    stopwatch.Restart();

                    encoder.WriteVideoFrame(imageQueue.Dequeue());
                    encodedFrameCount++;
                    stopwatch.Stop();
                    updateData((float)encodedFrameCount / totalFrameCount,
                                false,
                                stopwatch.Elapsed.TotalSeconds * (totalFrameCount - encodedFrameCount));
                }

                if (lastFrame && imageQueue.Count == 0) {
                    finalize();
                    finished = true;
                    break;
                }
            }
        }

        public void finalize() {
            encoder.Close();
        }

        private void updateData(float progress, bool finished, double eta) {

            if (update != null) {
                update(progress, finished, eta);
            }
        }

        public void writeToFrameBuffer(Bitmap frameToWrite) {
            imageQueue.Enqueue(frameToWrite);
        }


        public void writeFrame(Bitmap frameToWrite) {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            encoder.WriteVideoFrame(frameToWrite);
            encodedFrameCount++;
            stopwatch.Stop();
            updateData((float)encodedFrameCount / totalFrameCount,
                        false,
                        stopwatch.Elapsed.TotalSeconds * (totalFrameCount - encodedFrameCount));
        }
    }
}
