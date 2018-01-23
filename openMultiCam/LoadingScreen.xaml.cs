using System;
using System.Windows;
using System.Windows.Threading;
using openMultiCam.Utils;
using openMultiCam.Main.Encoder;
using System.Diagnostics;

namespace openMultiCam {
    /// <summary>
    /// Interaction logic for LoadingScreen.xaml
    /// </summary>
    public partial class LoadingScreen : Window {
        private float progressValue;
        private EncodingUtilities encodingUtilities;
        private string currentMessage;

        public LoadingScreen(String title, String message, EncodingUtilities encodingUtilities) {
            InitializeComponent();
            progressValue = 0;

            setStatusMessage(message);
            currentMessage = message;
            Title = title;
            Show();


            this.encodingUtilities = encodingUtilities;

        }

        public void startEncodingAs(EncodingUtilities.EncodingType encodingType) {

            if(encodingType == EncodingUtilities.EncodingType.GIF) {
                encodingUtilities.update += updateProgress;
                encodingUtilities.encodeAsGif();
            } else if(encodingType == EncodingUtilities.EncodingType.VP9) {
                encodingUtilities.update += updateProgress;
                encodingUtilities.encodeAsVP9();
            }

        }

        private void updateProgress(float progress, bool finished, double eta) {
            this.Dispatcher.Invoke(() => {
                setProgress(progress * 100f);


                setStatusMessage(currentMessage + " ETA: " + formatTime(eta));

                if (finished) {
                    this.Close();
                }
            });

        }

        private string formatTime(double timeInSeconds) {
            int hours = (int)(timeInSeconds / 3600);
            int minutes = (int)((timeInSeconds % 3600) / 60);
            int seconds = (int)((timeInSeconds % 3600) % 60);

            if (hours > 0) {
                return hours + "h"+ minutes + "m" + seconds + "s";
            } else {
                return minutes + "m" + seconds + "s";
            }
        }


        public void setProgress(float progressValue) {
            if (progressValue > 100) {
                this.progressValue = 100;
            } else if (progressValue < 0) {
                this.progressValue = 0;
            } else {
                this.progressValue = progressValue;
            }

            progressBackground.Width = this.Width * (progressValue / 100);
            progressLabel.Content = progressValue.ToString("0.00") + " %";
        }

        public void setStatusMessage(String message) {
            messageField.Text = message;
        }
    }
}
