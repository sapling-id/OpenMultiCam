using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Threading;
using openMultiCam.Utils;
using openMultiCam.WorkspaceUtils;
using System.Drawing.Imaging;
using System.Drawing;
using System.Windows.Threading;
using System.ComponentModel;

namespace openMultiCam {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        private Application currentApplication;
        private ScreenCaptureUtilities screenCaptureUtils;
        private ScreenRecorder screenRecorder;
        public static bool recording;
        public static float currentRecordingFramerate;
        public static float currentTargetFramerate;
        private RecordingArea recordingArea;
        public static System.Windows.Point recordingAreaOffset { get; private set; }
        public static bool leftDock { get; private set; }
        private delegate void UpdateUiOnRecording();
        private Thread updateCurrentFramerateThread;
        private System.Windows.Controls.Image recordActiveButtonImage;
        private System.Windows.Controls.Image recordInActiveButtonImage;
        private RecordingGallery recordingGallery;
        private bool galleryShowFlag;


        public MainWindow(Application currentApplication) {
            this.currentApplication = currentApplication;
            InitializeComponent();

            //initialize global recording properties
            MainWindow.recording = false;
            MainWindow.currentRecordingFramerate = 0;

            //initialize local recording and misc properties
            currentFrameRateLabel.Content = "";
            currentTargetFramerate = 50;
            galleryShowFlag = false;

            //buffer recording button images
            recordActiveButtonImage = new System.Windows.Controls.Image();
            recordActiveButtonImage.Source = new BitmapImage(new Uri(@"pack://application:,,,/Resources/omc_record_active.png"));
            recordActiveButtonImage.Width = 63;
            recordActiveButtonImage.Height = 15;
            recordInActiveButtonImage = new System.Windows.Controls.Image();
            recordInActiveButtonImage.Source = new BitmapImage(new Uri(@"pack://application:,,,/Resources/omc_record_inactive.png"));
            recordInActiveButtonImage.Width = 63;
            recordInActiveButtonImage.Height = 15;

            //initialize recording area and docking position
            recordingAreaOffset = new System.Windows.Point(5, 0);
            leftDock = false;
            leftLockButton.IsEnabled = true;
            rightLockButton.IsEnabled = false;

            //initialize the recorder
            screenCaptureUtils = new CopyScreenCapture();
            recordingArea = new RecordingArea(this, screenCaptureUtils);
            screenRecorder = new ScreenRecorder(screenCaptureUtils);

            //initialize recording gallery
            recordingGallery = new RecordingGallery();

            //initialize thread to update the current framerate
            updateCurrentFramerateThread = new Thread(updateCurrentFramerate);
            updateCurrentFramerateThread.Start();

            defaultFrameRateLabels();

        }

        private void toggleRecord(object sender, RoutedEventArgs e) {
            if(MainWindow.recording) {
                MainWindow.recording = false;
                recordingArea.setNonRecording();
                currentFrameRateLabel.Content = "";
                recordButton.Content = recordInActiveButtonImage;
                if(galleryShowFlag) {
                    recordingGallery.refreseh();
                }
            } else {
                MainWindow.recording = true;
                recordingArea.setRecording();
                currentFrameRateLabel.Content = "";
                recordButton.Content = recordActiveButtonImage;
            }
        }

        private void omc_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            updateCurrentFramerateThread.Abort();
            updateCurrentFramerateThread = null;
            currentApplication.Shutdown();
        }

        private void omc_LocationChanged(object sender, EventArgs e) {
            if(recordingArea.lockedCollider == "") {
                if (leftDock) {
                    recordingArea.Left = this.Left + this.Width + recordingAreaOffset.X;
                    recordingArea.Top = this.Top;
                } else {
                    recordingArea.Left = this.Left - (recordingArea.Width + recordingAreaOffset.X);
                    recordingArea.Top = this.Top;
                }
            }
        }

        private void omc_Loaded(object sender, RoutedEventArgs e) {
            if(leftDock) {
                recordingArea.Left = this.Left + this.Width + recordingAreaOffset.X;
                recordingArea.Top = this.Top;
            } else {
                recordingArea.Left = this.Left - (recordingArea.Width + recordingAreaOffset.X);
                recordingArea.Top = this.Top;
            }

            recordingArea.Show();
        }

        private void exitApplicationButton_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }

        private void leftLockButton_Click(object sender, RoutedEventArgs e) {
            toggleDockButtons();
        }

        private void rightLockButton_Click(object sender, RoutedEventArgs e) {
            toggleDockButtons();
        }

        private void toggleDockButtons() {
            if(leftDock) {
                leftDock = false;
                leftLockButton.IsEnabled = true;
                rightLockButton.IsEnabled = false;
                recordingArea.enableToggle(true);
                recordingArea.pushMainWindowHorizontally();
                recordingArea.enableToggle(false);
            } else {
                leftDock = true;
                leftLockButton.IsEnabled = false;
                rightLockButton.IsEnabled = true;
                recordingArea.enableToggle(true);
                recordingArea.pushMainWindowHorizontally();
                recordingArea.enableToggle(false);
            }
        }

        private void WindowMouseDown(object sender, MouseButtonEventArgs e) {
            this.DragMove();
        }

        private void omc_MouseUp(object sender, MouseButtonEventArgs e) {
            recordingArea.updateScreenCaptureUtils();
        }

        private void customButton_MouseEnter(object sender, MouseEventArgs e) {
            ((Button)sender).Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(80, 116, 116, 116));
        }

        private void customButton_MouseLeave(object sender, MouseEventArgs e) {
            ((Button)sender).Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(0, 0, 0, 0));
        }

        private void updateCurrentFramerate() {
            while(true) {
                if (MainWindow.recording) {
                    currentFrameRateLabel.Dispatcher.Invoke(new UpdateUiOnRecording(updateUiOnRecording));
                }
                Thread.Sleep(300);
            }
        }

        private void updateUiOnRecording() {
            currentFrameRateLabel.Content = (int)currentRecordingFramerate;
            currentFrameRateLabel.Foreground = new SolidColorBrush(ColorUtilities.getTintByPercentage(currentRecordingFramerate / currentTargetFramerate));
        }

        private void targetFrameRate_TextChanged(object sender, TextChangedEventArgs e) {
            int temporaryTargetFrameRate;
            bool isNumeric = int.TryParse(targetFrameRate.Text, out temporaryTargetFrameRate);
            if(isNumeric) {
                if(temporaryTargetFrameRate < 0) {
                    defaultFrameRateLabels();
                } else {
                    if (screenRecorder != null) {
                        screenRecorder.setFrameTime((int)(1000d / temporaryTargetFrameRate));
                    }
                    currentTargetFramerate = temporaryTargetFrameRate;
                }

            } else {
                defaultFrameRateLabels();
            }
        }

        private void defaultFrameRateLabels() {
            if (screenRecorder != null) {
                screenRecorder.setFrameTime(16);
            }
            targetFrameRate.Text = "60";
            currentTargetFramerate = 60;
        }

        private void recordingGalleryMenuButton_Click(object sender, RoutedEventArgs e) {
            if(!galleryShowFlag && !MainWindow.recording) {
                recordingGallery.refreseh();
                recordingGallery.Show();
                galleryShowFlag = true;
            } else {
                recordingGallery.Hide();
                galleryShowFlag = false;
            }

        }

        private void qualitySlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
            BitmapUtilities.setEncoderQuality((int)qualitySlider.Value);
        }
    }
}
