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
using System.Windows.Shapes;
using openMultiCam.Utils;

namespace openMultiCam {
    /// <summary>
    /// Interaction logic for recording area
    /// </summary>
    public partial class RecordingArea : Window {
        private static readonly int minSize = 50;
        private bool windowEventInitialized;
        private Point oldMousePosition;
        private Point currentMousePosition;
        public String lockedCollider { private set; get; }
        private Window mainWindow;
        private ScreenCaptureUtilities screenCaptureUtils;

        public RecordingArea(Window mainWindow, ScreenCaptureUtilities screenCaptureUtils) {
            InitializeComponent();

            this.mainWindow = mainWindow;
            this.screenCaptureUtils = screenCaptureUtils;

            windowEventInitialized = false;
            lockedCollider = "";
            setNonRecording();
            updateScreenCaptureUtils();
        }

        private void GeneralRectangleMouseDown(object sender, MouseButtonEventArgs e) {
            if (!windowEventInitialized) {
                oldMousePosition = PointToScreen(e.GetPosition(this));
                lockedCollider = (sender as Rectangle).Name;
                (sender as Rectangle).CaptureMouse();
            }
            windowEventInitialized = true;
        }

        public void setRecording() {
            BlendBorder.Stroke = new SolidColorBrush(Color.FromArgb(180, 255, 0, 0));
            this.Background = null;
        }

        public void setNonRecording() {
            BlendBorder.Stroke = new SolidColorBrush(Color.FromArgb(255, 0, 180, 250));
            this.Background = new SolidColorBrush(Color.FromArgb(2, 255, 255, 255));
        }

        public void pushMainWindowHorizontally() {
            if (MainWindow.leftDock) {
                mainWindow.Left = this.Left - mainWindow.Width - MainWindow.recordingAreaOffset.X;
            } else {
                mainWindow.Left = this.Left + this.Width + MainWindow.recordingAreaOffset.X;
            }
        }

        public void pushMainWindowVertically() {
            mainWindow.Top = this.Top;
        }

        public void enableToggle(bool value) {
            if(value) {
                lockedCollider = "toggle";
            } else {
                lockedCollider = "";
            }

        }

        private void GeneralRectangleMouseMove(object sender, MouseEventArgs e) {
            if (windowEventInitialized && !MainWindow.recording) {
                currentMousePosition = PointToScreen(e.GetPosition(this));
                double distanceX = currentMousePosition.X - oldMousePosition.X;
                double distanceY = currentMousePosition.Y - oldMousePosition.Y;
                oldMousePosition = PointToScreen(e.GetPosition(this));
                switch (lockedCollider) {
                    case "TopBorder":
                    {
                        if ((this.Height - distanceY) < minSize) {
                            this.Height = minSize;
                        } else {
                            this.Height = this.Height - distanceY;
                            this.Top = this.Top + distanceY;

                            pushMainWindowVertically();
                        }
                    }
                    break;
                    case "BottomBorder":
                    {
                        if ((this.Height + distanceY) < minSize) {
                            this.Height = minSize;
                        } else {
                            this.Height = this.Height + distanceY;
                        }

                    }
                    break;
                    case "LeftBorder":
                    {
                        if ((this.Width - distanceX) < minSize) {
                            this.Width = minSize;
                        } else {
                            this.Width = this.Width - distanceX;
                            this.Left = this.Left + distanceX;

                            pushMainWindowHorizontally();
                        }


                    }
                    break;
                    case "RightBorder":
                    {
                        if ((this.Width + distanceX) < minSize) {
                            this.Width = minSize;
                        } else {
                            this.Width = this.Width + distanceX;

                            pushMainWindowHorizontally();
                        }

                    }
                    break;
                    case "BottomLeftBorder":
                    {
                        if ((this.Width - distanceX) < minSize) {
                            this.Width = minSize;
                        } else {
                            this.Width = this.Width - distanceX;
                            this.Left = this.Left + distanceX;

                            pushMainWindowHorizontally();
                        }

                        if ((this.Height + distanceY) < minSize) {
                            this.Height = minSize;
                        } else {
                            this.Height = this.Height + distanceY;
                        }
                    }
                    break;
                    case "BottomRightBorder":
                    {
                        if ((this.Width + distanceX) < minSize) {
                            this.Width = minSize;
                        } else {
                            this.Width = this.Width + distanceX;

                            pushMainWindowHorizontally();
                        }

                        if ((this.Height + distanceY) < minSize) {
                            this.Height = minSize;
                        } else {
                            this.Height = this.Height + distanceY;
                        }
                    }
                    break;
                    case "TopLeftBorder":
                    {
                        if ((this.Width - distanceX) < minSize) {
                            this.Width = minSize;
                        } else {
                            this.Width = this.Width - distanceX;
                            this.Left = this.Left + distanceX;

                            pushMainWindowHorizontally();
                        }

                        if ((this.Height - distanceY) < minSize) {
                            this.Height = minSize;
                        } else {
                            this.Height = this.Height - distanceY;
                            this.Top = this.Top + distanceY;

                            pushMainWindowVertically();
                        }
                    }
                    break;
                    case "TopRightBorder":
                    {
                        if ((this.Width + distanceX) < minSize) {
                            this.Width = minSize;
                        } else {
                            this.Width = this.Width + distanceX;

                            pushMainWindowHorizontally();
                        }

                        if ((this.Height - distanceY) < minSize) {
                            this.Height = minSize;
                        } else {
                            this.Height = this.Height - distanceY;
                            this.Top = this.Top + distanceY;

                            pushMainWindowVertically();
                        }
                    }
                    break;
                    case "DragHighlighter":
                    {
                        this.Left = this.Left + distanceX;
                        this.Top = this.Top + distanceY;

                        pushMainWindowHorizontally();
                        pushMainWindowVertically();
                    }
                    break;
                    default:
                    break;
                }
            }
        }

        private void GeneralRectangleMouseUp(object sender, MouseButtonEventArgs e) {
            windowEventInitialized = false;
            lockedCollider = "";
            (sender as Rectangle).ReleaseMouseCapture();
            updateScreenCaptureUtils();
        }

        private void DragHighlighterMouseEnter(object sender, MouseEventArgs e) {
            if (!MainWindow.recording) {
                DragHighlighter.Visibility = Visibility.Visible;
            } else {
                DragHighlighter.Visibility = Visibility.Hidden;
            }
        }

        private void DragHighlighterMouseLeave(object sender, MouseEventArgs e) {
            DragHighlighter.Visibility = Visibility.Hidden;

            TopBorder.Opacity = 0.01;
            BottomBorder.Opacity = 0.01;
            LeftBorder.Opacity = 0.01;
            RightBorder.Opacity = 0.01;
            TopLeftBorder.Opacity = 0.01;
            TopRightBorder.Opacity = 0.01;
            BottomLeftBorder.Opacity = 0.01;
            BottomRightBorder.Opacity = 0.01;
        }

        private void RectangleHighlightMouseEnter(object sender, MouseEventArgs e) {
            if (!MainWindow.recording) {
                (sender as Rectangle).Opacity = 0.35;
            } else {
                (sender as Rectangle).Opacity = 0.01;
            }
        }

        private void RectangleHighlightMouseLeave(object sender, MouseEventArgs e) {
            (sender as Rectangle).Opacity = 0.01;
        }

        public void updateScreenCaptureUtils() {
            screenCaptureUtils.width = (int)this.Width - 3;
            screenCaptureUtils.height = (int)this.Height - 4;
            screenCaptureUtils.xOffset = (int)this.Left + 2;
            screenCaptureUtils.yOffset = (int)this.Top + 2; ;
        }
    }
}
