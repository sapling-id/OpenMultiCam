using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace openMultiCam.Utils {
    public class VideoFrameSerializer {
        
        private String filePath;
        private XmlWriterSettings xmlWriterSettings;
        private XmlWriter xmlWriter;

        public VideoFrameSerializer(String filePath) {
            this.filePath = filePath;
            xmlWriterSettings = new XmlWriterSettings();
            xmlWriterSettings.Indent = true;
            xmlWriterSettings.IndentChars = "\t";
            initializeWriter();
        }

        private void initializeWriter() {
            xmlWriter = XmlWriter.Create(filePath + CamConstants.VIDEO_FRAMES_FILENAME, xmlWriterSettings);
            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement(VideoXMLConstants.VFRAMES_VIDEOFRAMES_TAG);
        }

        public void writeFrame(byte[] frameToWrite, int index) {
            xmlWriter.WriteStartElement(VideoXMLConstants.VFRAMES_FRAME_TAG);
            xmlWriter.WriteAttributeString(VideoXMLConstants.VFRAMES_ATTRIBUTE_ID, index.ToString());
            xmlWriter.WriteAttributeString(VideoXMLConstants.VFRAMES_ATTRIBUTE_LENGTH, frameToWrite.Length.ToString());
            xmlWriter.WriteBinHex(frameToWrite, 0, frameToWrite.Length);
            xmlWriter.WriteEndElement();
        }

        public void finalize() {
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndDocument();
            xmlWriter.Dispose();
            xmlWriter.Close();
        }
    }
}
