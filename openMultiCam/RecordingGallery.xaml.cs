using openMultiCam.GuiCustoms;
using openMultiCam.Utils;
using openMultiCam.WorkspaceUtils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace openMultiCam {
    /// <summary>
    /// Interaction logic for Recordings.xaml
    /// </summary>
    public partial class RecordingGallery : Window {
        private WorkspaceManager workspaceManager;

        public RecordingGallery() {
            InitializeComponent();

            workspaceManager = new WorkspaceManager();
            refreseh();
        }

        public void refreseh() {
            workspaceManager.indexWorkspace();
            recordingCountLabel.Content = workspaceManager.getPreviews().Length;
            gallerySizeLabel.Content = workspaceManager.getFormattedTotalWorkspaceSizeInGigaByte();

            loadPreviewItems();
            System.GC.Collect();
        }

        private void loadPreviewItems() {
            VideoPreview[] videoPreviews = workspaceManager.getPreviews();
            ObservableCollection<VideoPreviewItemModel> videoPreviewItemModels = new ObservableCollection<VideoPreviewItemModel>();

            foreach (VideoPreview currentPreview in videoPreviews) {
                VideoPreviewItemModel currentVideoPreviewItemModel = new VideoPreviewItemModel();
                currentVideoPreviewItemModel.ownVideoPreviewInstance = currentPreview;
                System.Drawing.Image currentImage = currentPreview.previewImage;

                if(currentImage != null) {
                    BitmapImage bitmapImage = new BitmapImage();
                    using (MemoryStream memory = new MemoryStream()) {
                        currentImage.Save(memory, ImageFormat.Png);
                        memory.Position = 0;

                        bitmapImage.BeginInit();
                        bitmapImage.StreamSource = memory;
                        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                        bitmapImage.EndInit();
                    }

                    currentVideoPreviewItemModel.PreviewImage = bitmapImage;
                    currentVideoPreviewItemModel.PlayTime = currentPreview.playTime.getFormattedTime();
                    currentVideoPreviewItemModel.ForegroundColor = new SolidColorBrush(ColorUtilities.getTintByPercentage(currentPreview.recordingQuality));
                    currentVideoPreviewItemModel.RecordingQuality = (int)(currentPreview.recordingQuality*100) + "%";
                    currentVideoPreviewItemModel.ParentWindow = this;
                    videoPreviewItemModels.Add(currentVideoPreviewItemModel);
                }


            }

            listBoxGallery.ItemsSource = videoPreviewItemModels;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            e.Cancel = true;
            this.Hide();
        }
    }
}
