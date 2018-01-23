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

        private void exportButton_Click(object sender, RoutedEventArgs e) {
            VideoPreviewItemModel itemModel = (VideoPreviewItemModel)(sender as Button).DataContext;
            VideoFileReader videoFileReader = new VideoFileReader(WorkspaceManager.getWorkspacePathForGivenFile(itemModel.ownVideoPreviewInstance.fileName));
            LoadingScreen loadingScreen = new LoadingScreen("Encoding as gif...", "Please, stand by!", new EncodingUtilities(videoFileReader.videoFileMetaData.filePath));
            loadingScreen.startEncodingAsGif();
            /*MessageBox.Show("button works! " + itemModel.ownVideoPreviewInstance.fileName + "\n " + itemModel.ownVideoPreviewInstance.filePath);

            Bitmap bmp = videoFileReader.getFrameAtIndex(50);

            //Bitmap bmp = videoFileReader.getNextFrame();

            using (MemoryStream memory = new MemoryStream()) {
                Bitmap bmp2 = new Bitmap(bmp);
                bmp2.Save(memory, ImageFormat.Png);
                memory.Position = 0;
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();

                item.PreviewImage = bitmapImage;
            }*/
            //bmp.Save(WorkspaceManager.getWorkspacePathForGivenFile(itemModel.ownVideoPreviewInstance.fileName) + "\\testImage.jpeg", ImageFormat.Jpeg);
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
