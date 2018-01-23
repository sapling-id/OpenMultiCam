using openMultiCam.WorkspaceUtils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using static System.Net.Mime.MediaTypeNames;

namespace openMultiCam.GuiCustoms {
    public class VideoPreviewItemModel: INotifyPropertyChanged {

        public VideoPreview ownVideoPreviewInstance { get; set; }
        public RecordingGallery ParentWindow { get; set; }
        private ImageSource PrevieImage_ { get; set; }
        public ImageSource PreviewImage {
            get {
                return PrevieImage_;
            }
            set {
                PrevieImage_ = value;
                OnPropertyChanged("PreviewImage");
            }
        }
        public String PlayTime { get; set; }
        public String RecordingQuality { get; set; }
        private System.Windows.Media.Brush ForegroundColor_ { get; set; }
        public System.Windows.Media.Brush ForegroundColor
        {
            get { return ForegroundColor_; }
            set
            {
                ForegroundColor_ = value;
                OnPropertyChanged("ForegroundColor");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
