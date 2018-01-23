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

        public void startEncodingAsGif() {

            encodingUtilities.update += updateProgress;
            encodingUtilities.encodeAsGif();
        }

        private void updateProgress() {
            this.Dispatcher.Invoke(() => {
                setProgress(encodingUtilities.encodingProgress * 100f);


                setStatusMessage(currentMessage + " ETA: " + formatTime(encodingUtilities.estimatedTimeOfArrival));

                if (encodingUtilities.encodingFinished) {
                    this.Close();
                }
            });

        }

        private string formatTime(double timeInSeconds) {
            int seconds = (int)(encodingUtilities.estimatedTimeOfArrival - (encodingUtilities.estimatedTimeOfArrival / 60d));
            int minutes = (int)(encodingUtilities.estimatedTimeOfArrival / 60d);
            int hours = (int)(encodingUtilities.estimatedTimeOfArrival / 60d / 60d);

            if(hours > 0) {
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
