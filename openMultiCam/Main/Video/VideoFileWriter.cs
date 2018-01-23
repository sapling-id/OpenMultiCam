using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace openMultiCam.Utils {
    public class VideoFileWriter {
        public const String VIDEO_PREVIEW_FILENAME = "vPreview.jpeg";
        public const String VIDEO_META_DATA_FILENAME = "metaData.xml";
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

            xmlWriterSettings = new XmlWriterSettings();
            xmlWriterSettings.Indent = true;
            xmlWriterSettings.IndentChars = "\t";
        }

        public void writeFrame(Bitmap frameToWrite) {
            frameCount++;
            if (initialFrame) {
                initialFrame = false;
                isOpen = true;
                frameToWrite.Save(filePath + VideoFileWriter.VIDEO_PREVIEW_FILENAME, BitmapUtilities.jpegEncoder, BitmapUtilities.encodingParameters);
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
