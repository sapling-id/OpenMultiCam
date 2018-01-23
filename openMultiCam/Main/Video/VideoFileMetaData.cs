using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace openMultiCam.Utils {
    public class VideoFileMetaData {
        public string filePath { get; private set; }
        public float targetFramerate { get; private set; }
        public float averageFramerate { get; private set; }
        public int frameCount { get; private set; }
        public int frameSize { get; private set; }
        public int frameWidth { get; private set; }
        public int frameHeight { get; private set; }
        public float recordingQuality { get; private set; }

        public VideoFileMetaData(string filePath) {
            this.filePath = filePath;
            loadMetaDataFromXML();
        }

        private void loadMetaDataFromXML() {
            if (File.Exists(filePath + VideoFileWriter.VIDEO_META_DATA_FILENAME)) {
                using (XmlReader xmlReader = XmlReader.Create(filePath + VideoFileWriter.VIDEO_META_DATA_FILENAME)) {
                    while (xmlReader.Read()) {
                        if (xmlReader.IsStartElement()) {
                            switch (xmlReader.Name) {
                                case VideoXMLConstants.META_AVERAGEFRAMERATE_TAG:
                                    xmlReader.Read();
                                    this.averageFramerate = float.Parse(xmlReader.Value.Trim());
                                    break;
                                case VideoXMLConstants.META_TARGETFRAMERATE_TAG:
                                    xmlReader.Read();
                                    this.targetFramerate = float.Parse(xmlReader.Value.Trim());
                                    break;
                                case VideoXMLConstants.META_FRAMECOUNT_TAG:
                                    xmlReader.Read();
                                    this.frameCount = int.Parse(xmlReader.Value.Trim());
                                    break;
                                case VideoXMLConstants.META_RECORDINGQUALITY_TAG:
                                    xmlReader.Read();
                                    this.recordingQuality = float.Parse(xmlReader.Value.Trim());
                                    break;
                                case VideoXMLConstants.META_FRAMEWIDTH_TAG:
                                    xmlReader.Read();
                                    this.frameWidth = int.Parse(xmlReader.Value.Trim());
                                    break;
                                case VideoXMLConstants.META_FRAMEHEIGHT_TAG:
                                    xmlReader.Read();
                                    this.frameHeight = int.Parse(xmlReader.Value.Trim());
                                    break;
                                default: break;
                            }
                        }
                    }
                }
            }

        }
    }
}
