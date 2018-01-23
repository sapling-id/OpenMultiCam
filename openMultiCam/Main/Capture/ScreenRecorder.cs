using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using openMultiCam.Utils;
using System.Security.AccessControl;

namespace openMultiCam {
    public class ScreenRecorder {
        private Stopwatch stopwatch;
        private long oldTime;
        private long timeDelta;
        private ScreenCaptureUtilities screenCaptureUtils;
        private Thread recordingThread;
        private bool videoWriterFlag;
        private String workingDirectory;
        private float averageFramerate;
        private float framerateBuffer;
        private bool framerateBufferInitialization;
        public static int frameTime { get; private set; }
        private VideoFileWriter videoFileWriter;

        public ScreenRecorder(ScreenCaptureUtilities screenCaptureUtils) {
            this.screenCaptureUtils = screenCaptureUtils;
            stopwatch = new Stopwatch();

            oldTime = 0;
            timeDelta = 0;
            frameTime = 20;
            averageFramerate = 0;
            framerateBuffer = 0;
            framerateBufferInitialization = true;
            videoWriterFlag = true;

            recordingThread = new Thread(recordThread);
            recordingThread.IsBackground = true;
            recordingThread.Start();
        }

        public void setFrameTime(int milliseconds) {
            if(frameTime < 15) {
                frameTime = 15;
            } else {
                frameTime = milliseconds;
            }

        }

        private void recordThread() {
            stopwatch.Start();
            while(true) {
                if (MainWindow.recording) {
                    if(videoWriterFlag) {
                        reset();
                    }
                    //Stopwatch stopWatch = new Stopwatch();
                    //stopWatch.Start();
                    //screenCaptureUtils.captureScreen();
                    //stopWatch.Stop();
                    //Debug.WriteLine("frameTime > " + stopWatch.Elapsed.TotalSeconds + " s");

                    videoFileWriter.writeToFrameBuffer(screenCaptureUtils.captureScreen());

                    timeDelta = stopwatch.ElapsedMilliseconds - oldTime;
                    if (timeDelta < frameTime) {
                        Thread.Sleep((int)(frameTime - timeDelta));
                        MainWindow.currentRecordingFramerate = 1000f / (timeDelta + (frameTime - timeDelta));
                    } else {
                        MainWindow.currentRecordingFramerate = 1000f / timeDelta;
                    }
                    calculateAverageFramerate();
                    oldTime = stopwatch.ElapsedMilliseconds;
                } else {
                    videoWriterFlag = true;
                    if(videoFileWriter != null && videoFileWriter.isOpen) {
                        videoFileWriter.lastFrame = true;
                        while(!videoFileWriter.finished) {
                            Thread.Sleep(30);
                        }
                        Debug.WriteLine("finalized");
                        videoFileWriter.finalize(averageFramerate);
                    }
                    Thread.Sleep(100);
                }
            }
        }

        private void reset() {
            averageFramerate = 0;
            framerateBuffer = 0;
            framerateBufferInitialization = true;

            videoWriterFlag = false;

            workingDirectory = CamConstants.WORKSPACE_PATH + UniqueFileName.generate() + "\\";
            DirectoryInfo directoryInfo = Directory.CreateDirectory(workingDirectory);

            videoFileWriter = new VideoFileWriter(workingDirectory, 1000 / frameTime, screenCaptureUtils.width, screenCaptureUtils.height);
        }

        private void calculateAverageFramerate() {
            framerateBuffer = MainWindow.currentRecordingFramerate;
            if (framerateBufferInitialization) {
                framerateBufferInitialization = false;
                averageFramerate = framerateBuffer;
            } else {
                averageFramerate = (averageFramerate + framerateBuffer) / 2;
            }
        }
    }
}
