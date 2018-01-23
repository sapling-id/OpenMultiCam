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
    public class VideoFrameDeserializer {
        private VideoFileMetaData videoFileMetaData;
        XmlReader staticXmlReader;

        public VideoFrameDeserializer(VideoFileMetaData videoFileMetaData) {
            this.videoFileMetaData = videoFileMetaData;
            reset();
        }

        public void reset() {
            FileStream fileStream = new FileStream(videoFileMetaData.filePath + CamConstants.VIDEO_FRAMES_FILENAME, FileMode.Open,FileAccess.Read, FileShare.ReadWrite);
            staticXmlReader = XmlReader.Create(fileStream);
        }

        public void dispose() {
            staticXmlReader.Dispose();
            staticXmlReader.Close();
        }

        public Bitmap getNextFrame() {
            if (staticXmlReader != null) {
                while (staticXmlReader.Read()) {
                    if (staticXmlReader.IsStartElement()) {
                        if (staticXmlReader.Name == VideoXMLConstants.VFRAMES_FRAME_TAG) {
                            int length;
                            bool isNumeric = int.TryParse(staticXmlReader.GetAttribute(VideoXMLConstants.VFRAMES_ATTRIBUTE_LENGTH), out length);
                            staticXmlReader.Read();

                            if(isNumeric) {
                                byte[] serializedBitmap = new byte[length];
                                staticXmlReader.ReadContentAsBinHex(serializedBitmap, 0, length);
                                return BitmapUtilities.byteArrayToBitmap(serializedBitmap);
                            } else {
                                return null;
                            }

                        }
                    }
                }
            }
            return null;
        }

        public static Bitmap loadFrameAtIndex(String filePath, int index) {
            if (File.Exists(filePath + CamConstants.VIDEO_FRAMES_FILENAME)) {
                using (XmlReader xmlReader = XmlReader.Create(new FileStream(filePath + CamConstants.VIDEO_FRAMES_FILENAME, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))) {
                    while (xmlReader.Read()) {
                        if (xmlReader.IsStartElement()) {
                            if (xmlReader.Name == VideoXMLConstants.VFRAMES_FRAME_TAG) {
                                if (xmlReader.GetAttribute(VideoXMLConstants.VFRAMES_ATTRIBUTE_ID) == index.ToString()) {
                                    int length;
                                    bool isNumeric = int.TryParse(xmlReader.GetAttribute(VideoXMLConstants.VFRAMES_ATTRIBUTE_LENGTH), out length);
                                    xmlReader.Read();

                                    if (isNumeric) {
                                        byte[] serializedBitmap = new byte[length];
                                        xmlReader.ReadContentAsBinHex(serializedBitmap, 0, length);
                                        return BitmapUtilities.byteArrayToBitmap(serializedBitmap);
                                    } else {
                                        return null;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return null;
        }
    }
}
