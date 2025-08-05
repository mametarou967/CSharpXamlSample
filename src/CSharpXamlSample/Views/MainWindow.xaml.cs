using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CSharpXamlSample.Views
{
    public partial class MainWindow : Window
    {
        private const double LineHeight = 24;
        private const int VisibleLines = 6;
        private const double PaddingTopBottom = 4;
        private const double ScrollViewerHeight = LineHeight * VisibleLines + PaddingTopBottom * 2;

        private int _currentLine = 0;
        private bool _draggingSlider = false;
        private double _sliderOffsetY;

        private Point _startDragPoint;
        private int _startLine;

        private DateTime _pressStartTime;
        private bool _awaitingHoldStart = false;
        private UIElement _heldButton = null;

        public MainWindow()
        {
            InitializeComponent();

            MyScrollViewer.Height = ScrollViewerHeight;
            ((Border)MyScrollViewer.Parent).Height = ScrollViewerHeight;

            InitText();

            CompositionTarget.Rendering += OnFrameUpdate;
        }

        private void InitText()
        {
            for (int i = 1; i <= 50; i++)
            {
                TextPanel.Children.Add(new TextBlock
                {
                    Text = $"これはテキストの {i} 行目です。",
                    FontSize = 16,
                    Height = LineHeight,
                    Padding = new Thickness(0)
                });
            }
        }

        private void SetScrollToLine(int line)
        {
            int maxLine = TextPanel.Children.Count - VisibleLines;
            line = Math.Max(0, Math.Min(maxLine, line));

            if (line == _currentLine) return;

            MyScrollViewer.ScrollToVerticalOffset(line * LineHeight);
            _currentLine = line;
            UpdateSlider();
        }

        private void ScrollViewer_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _startDragPoint = e.GetPosition(this);
            _startLine = _currentLine;
            Mouse.Capture(MyScrollViewer);
        }

        private void ScrollViewer_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (Mouse.Captured == MyScrollViewer)
            {
                var current = e.GetPosition(this);
                double delta = _startDragPoint.Y - current.Y;
                int lineDelta = (int)Math.Round(delta / LineHeight);
                SetScrollToLine(_startLine + lineDelta);
            }
        }

        private void ScrollViewer_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Mouse.Capture(null);
        }

        private void UpdateSlider()
        {
            double scrollableHeight = MyScrollViewer.ExtentHeight - MyScrollViewer.ViewportHeight;
            if (scrollableHeight <= 0) return;

            double ratio = MyScrollViewer.VerticalOffset / scrollableHeight;

            double trackHeight = ((Grid)Slider.Parent).ActualHeight;
            double maxSliderTop = trackHeight - Slider.ActualHeight;
            double sliderTop = ratio * maxSliderTop;
            Slider.Margin = new Thickness(5, sliderTop, 5, 0);
        }

        private void Slider_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _draggingSlider = true;
            _sliderOffsetY = e.GetPosition(Slider).Y;
            Mouse.Capture(Slider);
        }

        private void Slider_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_draggingSlider) return;

            var track = (Grid)Slider.Parent;
            double posY = e.GetPosition(track).Y - _sliderOffsetY;
            double trackHeight = track.ActualHeight - Slider.ActualHeight;
            posY = Math.Max(0, Math.Min(posY, trackHeight));
            int maxLine = TextPanel.Children.Count - VisibleLines;
            int line = (int)Math.Round(posY / trackHeight * maxLine);
            SetScrollToLine(line);
        }

        private void Slider_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _draggingSlider = false;
            Mouse.Capture(null);
        }

        private void ScrollTrack_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var track = (Grid)sender;
            Point clickPos = e.GetPosition(track);
            double trackHeight = track.ActualHeight;
            double sliderHeight = Slider.ActualHeight;
            double posY = clickPos.Y - sliderHeight / 2;
            posY = Math.Max(0, Math.Min(posY, trackHeight - sliderHeight));
            Slider.Margin = new Thickness(5, posY, 5, 0);

            double ratio = posY / (trackHeight - sliderHeight);
            double offset = ratio * (MyScrollViewer.ExtentHeight - MyScrollViewer.ViewportHeight);
            MyScrollViewer.ScrollToVerticalOffset(offset);
            UpdateSlider();
        }

        // ▲▼ボタン押しっぱなし対応（CompositionTarget.Rendering）

        private bool _isScrollingHeld = false;
        private int _scrollHeldDirection = 0;
        private int _scrollFrameCounter = 0;

        private void OnFrameUpdate(object sender, EventArgs e)
        {
            if (_awaitingHoldStart)
            {
                if ((DateTime.Now - _pressStartTime).TotalMilliseconds >= 100)
                {
                    // 長押し判定で連続スクロール開始
                    _awaitingHoldStart = false;
                    _isScrollingHeld = true;
                    SetScrollToLine(_currentLine + _scrollHeldDirection); // 初回
                }
                return;
            }

            if (_isScrollingHeld)
            {
                _scrollFrameCounter++;
                if (_scrollFrameCounter % 2 == 0)
                {
                    SetScrollToLine(_currentLine + _scrollHeldDirection);
                }
            }
        }

        private void UpButton_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _pressStartTime = DateTime.Now;
            _scrollHeldDirection = -1;
            _scrollFrameCounter = 0;
            _awaitingHoldStart = true;
            _heldButton = (UIElement)sender;

            Mouse.Capture(_heldButton);
        }

        private void DownButton_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _pressStartTime = DateTime.Now;
            _scrollHeldDirection = 1;
            _scrollFrameCounter = 0;
            _awaitingHoldStart = true;
            _heldButton = (UIElement)sender;

            Mouse.Capture(_heldButton);
        }

        private void ScrollButton_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_awaitingHoldStart)
            {
                // クリック扱い：1行だけスクロール
                SetScrollToLine(_currentLine + _scrollHeldDirection);
            }

            _awaitingHoldStart = false;
            _isScrollingHeld = false;
            _heldButton = null;
            Mouse.Capture(null);
        }

        private void ScrollButton_LostMouseCapture(object sender, MouseEventArgs e)
        {
            _awaitingHoldStart = false;
            _isScrollingHeld = false;
            _heldButton = null;
        }

        private void UpButton_Click(object sender, RoutedEventArgs e)
        {
            SetScrollToLine(_currentLine - 1);
        }

        private void DownButton_Click(object sender, RoutedEventArgs e)
        {
            SetScrollToLine(_currentLine + 1);
        }
    }
}
