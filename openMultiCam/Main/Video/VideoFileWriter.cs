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
using System.Xml;

namespace openMultiCam.Utils {
    public class VideoFileWriter {
        public const String VIDEO_PREVIEW_FILENAME = "vPreview.jpeg";
        public const String VIDEO_META_DATA_FILENAME = "metaData.xml";
        public bool lastFrame { get; set; }
        public bool finished { get; set; }
        private String filePath;
        private float targetFramerate;
        private float averageFramerate;
        private int frameCount;
        private int frameWidth;
        private int frameHeight;
        private VideoFrameSerializer videoFrameSearializer;
        private bool initialFrame;
        public bool isOpen { get; private set; }
        private XmlWriterSettings xmlWriterSettings;
        private Queue<Bitmap> imageQueue;
        private Thread writingThread;

        public VideoFileWriter(String filePath, float targetFramerate, int frameWidth, int frameHeight) {
            this.filePath = filePath;
            this.targetFramerate = targetFramerate;
            this.frameWidth = frameWidth;
            this.frameHeight = frameHeight;
            this.frameCount = 0;
            this.averageFramerate = 0;
            this.isOpen = false;
            videoFrameSearializer = new VideoFrameSerializer(filePath);
            initialFrame = true;
            lastFrame = false;
            finished = false;

            xmlWriterSettings = new XmlWriterSettings();
            xmlWriterSettings.Indent = true;
            xmlWriterSettings.IndentChars = "\t";

            imageQueue = new Queue<Bitmap>();

            writingThread = new Thread(write);
            writingThread.Start();
        }

        public void writeToFrameBuffer(Bitmap frameToWrite) {
            imageQueue.Enqueue(frameToWrite);
        }

        private void write() {
            while (true) {
                if (imageQueue.Count > 0) {

                    writeFrame(imageQueue.Dequeue());
                }

                if (lastFrame && imageQueue.Count == 0) {
                    finished = true;
                    break;
                }
            }
        }

        private void writeFrame(Bitmap frameToWrite) {
            frameCount++;
            if (initialFrame) {
                initialFrame = false;
                isOpen = true;
                frameToWrite.Save(filePath + VideoFileWriter.VIDEO_PREVIEW_FILENAME, BitmapUtilities.pngCodecInfo, BitmapUtilities.encodingParameters);
            }
            videoFrameSearializer.writeFrame(BitmapUtilities.bitmapToByteArrayFromMemoryStream(frameToWrite), frameCount -1);
        }

        public void finalize(float averageFramerate) {
            videoFrameSearializer.finalize();
            this.averageFramerate = averageFramerate;
            saveMetaDataToXML();
            isOpen = false;
        }

        private void saveMetaDataToXML() {
            using(XmlWriter xmlWriter = XmlWriter.Create(filePath + VideoFileWriter.VIDEO_META_DATA_FILENAME, xmlWriterSettings)) {
                xmlWriter.WriteStartDocument();
                xmlWriter.WriteStartElement(VideoXMLConstants.META_DATA_TAG);

                xmlWriter.WriteElementString(VideoXMLConstants.META_FRAMECOUNT_TAG, frameCount.ToString());
                xmlWriter.WriteElementString(VideoXMLConstants.META_TARGETFRAMERATE_TAG, targetFramerate.ToString());
                xmlWriter.WriteElementString(VideoXMLConstants.META_AVERAGEFRAMERATE_TAG, averageFramerate.ToString());
                float temporaryRecordingQuality = averageFramerate / targetFramerate;
                if(temporaryRecordingQuality > 1) {
                    temporaryRecordingQuality = 1;
                }
                xmlWriter.WriteElementString(VideoXMLConstants.META_RECORDINGQUALITY_TAG, temporaryRecordingQuality.ToString());
                xmlWriter.WriteElementString(VideoXMLConstants.META_FRAMEWIDTH_TAG, frameWidth.ToString());
                xmlWriter.WriteElementString(VideoXMLConstants.META_FRAMEHEIGHT_TAG, frameHeight.ToString());

                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndDocument();
            }

        }
    }
}
