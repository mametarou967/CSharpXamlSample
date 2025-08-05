using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace CSharpXamlSample.Views
{
    public partial class MainWindow : Window
    {
        private double _velocity = 0;
        private DispatcherTimer _inertiaTimer;

        private bool _draggingSlider = false;
        private double _sliderOffsetY;

        private const double LineHeight = 24;
        private const int VisibleLines = 6;
        private const double ExtraPadding = 8; // 上下4pxずつ
        private const double ScrollViewerHeight = LineHeight * VisibleLines + ExtraPadding;

        public MainWindow()
        {
            InitializeComponent();
            MyScrollViewer.Height = ScrollViewerHeight;
            ((Border)MyScrollViewer.Parent).Height = ScrollViewerHeight;
            InitText();
        }
        private void InitText()
        {
            for (int i = 1; i <= 50; i++)
            {
                TextPanel.Children.Add(new TextBlock
                {
                    Text = $"これはテキストの {i} 行目です。",
                    FontSize = 16,
                    Height = LineHeight,        // ✅ 明示的に高さを固定
                    Padding = new Thickness(0)  // ✅ Paddingをなくす
                });
            }
        }

        private void SetScrollToLine(int line)
        {
            int maxLine = TextPanel.Children.Count - VisibleLines;
            line = Math.Max(0, Math.Min(maxLine, line));
            MyScrollViewer.ScrollToVerticalOffset(line * LineHeight);
            _currentLine = line;
            UpdateSlider();
        }
        private Point _startDragPoint;
        private int _startLine;

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

        private void InertiaScroll(object sender, EventArgs e)
        {
            if (Math.Abs(_velocity) < 0.5)
            {
                _inertiaTimer.Stop();
                StopInertiaAndSnap(); // ✅ 慣性終了後もスナップ
                return;
            }

            MyScrollViewer.ScrollToVerticalOffset(MyScrollViewer.VerticalOffset + _velocity * 0.016);
            _velocity *= 0.90;
            UpdateSlider();
        }

        private void Slider_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _draggingSlider = false;
            Mouse.Capture(null);
        }

        private void StopInertiaAndSnap()
        {
            double offset = MyScrollViewer.VerticalOffset;
            double snappedOffset = Math.Round(offset / LineHeight) * LineHeight;
            MyScrollViewer.ScrollToVerticalOffset(snappedOffset);
            UpdateSlider();
        }
        private int _currentLine = 0;

        private void UpButton_Click(object sender, RoutedEventArgs e)
        {
            SetScrollToLine(_currentLine - 1);
        }

        private void DownButton_Click(object sender, RoutedEventArgs e)
        {
            SetScrollToLine(_currentLine + 1);
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
    }
}
