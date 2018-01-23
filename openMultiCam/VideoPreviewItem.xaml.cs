using openMultiCam.GuiCustoms;
using openMultiCam.Main.Encoder;
using openMultiCam.Utils;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace openMultiCam {
    /// <summary>
    /// Interaction logic for VideoPreviewItem.xaml
    /// </summary>
    public partial class VideoPreviewItem : UserControl {
        public VideoPreviewItem() {
            InitializeComponent();
        }

        private void exportButtonGif_Click(object sender, RoutedEventArgs e) {
            VideoPreviewItemModel itemModel = (VideoPreviewItemModel)(sender as Button).DataContext;
            VideoFileReader videoFileReader = new VideoFileReader(WorkspaceManager.getWorkspacePathForGivenFile(itemModel.ownVideoPreviewInstance.fileName));
            LoadingScreen loadingScreen = new LoadingScreen("Encoding as gif...", "Please, stand by!", new EncodingUtilities(videoFileReader.videoFileMetaData.filePath, 100));

            loadingScreen.startEncodingAs(EncodingUtilities.EncodingType.GIF);
            videoFileReader.dispose();
            videoFileReader = null;

        }

        private void exportButtonVP9_Click(object sender, RoutedEventArgs e) {
            VideoPreviewItemModel itemModel = (VideoPreviewItemModel)(sender as Button).DataContext;
            VideoFileReader videoFileReader = new VideoFileReader(WorkspaceManager.getWorkspacePathForGivenFile(itemModel.ownVideoPreviewInstance.fileName));
            LoadingScreen loadingScreen = new LoadingScreen("Encoding as webm...", "Please, stand by!", new EncodingUtilities(videoFileReader.videoFileMetaData.filePath, 100));

            loadingScreen.startEncodingAs(EncodingUtilities.EncodingType.VP9);
            videoFileReader.dispose();
            videoFileReader = null;

        }

        private void deleteButton_Click(object sender, RoutedEventArgs e) {
            VideoPreviewItemModel itemModel = (VideoPreviewItemModel)(sender as Button).DataContext;
            DirectoryInfo directoryInfo = new DirectoryInfo(itemModel.ownVideoPreviewInstance.filePath);
            directoryInfo.Delete(true);
            itemModel.ParentWindow.refreseh();
        }

        private void customButton_MouseEnter(object sender, MouseEventArgs e) {
            ((Button)sender).Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(80, 116, 116, 116));
        }

        private void customButton_MouseLeave(object sender, MouseEventArgs e) {
            ((Button)sender).Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(0, 0, 0, 0));
        }


    }
}
