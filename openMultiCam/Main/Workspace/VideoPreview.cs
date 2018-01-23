using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openMultiCam.WorkspaceUtils {
    public class VideoPreview {
        public Image previewImage { get; private set;}
        public PlayTime playTime { get;  private set; }
        public String fileName { get; private set; }
        public String filePath { get; private set; }
        public float recordingQuality { get; private set; }
        private long byteLength;

        public VideoPreview(Image previewImage, PlayTime playTime, long byteLength, String fileName, String filePath, float recordingQuality) {
            resizeToVideoPreview(previewImage);
            this.playTime = playTime;
            this.fileName = fileName;
            this.byteLength = byteLength;
            this.filePath = filePath;
            this.recordingQuality = recordingQuality;
        }

        public float getFileSizeInMegaByte() {
            return byteLength / 1024f / 1024f;
        }

        public float getFileSizeInGigaByte() {
            return byteLength / 1024f / 1024f / 1024f;
        }

        public String getFormattedFileSizeInGigaByte() {
            return getFileSizeInGigaByte().ToString("0.00") + " GB";
        }

        private void resizeToVideoPreview(Image previewImage) {
            if(previewImage != null) {
                if (previewImage.Width > previewImage.Height) {
                    this.previewImage = (Image)(new Bitmap(previewImage, new Size(200, (int)(previewImage.Height * (200f / previewImage.Width)))));
                } else {
                    this.previewImage = (Image)(new Bitmap(previewImage, new Size((int)(previewImage.Width * (130f / previewImage.Height)), 130)));
                }
            }

        }
    }
}
