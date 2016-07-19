using System;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace SimpleDemo.UWP
{
    public sealed partial class UwpCustomControl : UserControl
    {
        private Rectangle iRectangle;

        public UwpCustomControl()
        {
            this.InitializeComponent();

            DrawRectangle();
        }

        /// <summary>
        /// Draws the rectangle with text.
        /// </summary>
        private void DrawRectangle()
        {
            var canvas = new Canvas();
            canvas.Height = 200;
            canvas.Width = 350;
            canvas.VerticalAlignment = VerticalAlignment.Top;
            canvas.HorizontalAlignment = HorizontalAlignment.Left;

            iRectangle = new Rectangle();
            iRectangle.Width = 100;
            iRectangle.Height = 40;
            iRectangle.Stroke = new SolidColorBrush(Colors.Red);
            iRectangle.Fill = new SolidColorBrush(Colors.Blue);
            iRectangle.StrokeThickness = 4;
            iRectangle.Margin = new Thickness(20, 20, 0, 0);
            iRectangle.HorizontalAlignment = HorizontalAlignment.Left;
            iRectangle.VerticalAlignment = VerticalAlignment.Top;

            var textBlock = new TextBlock();
            textBlock.Text = "Sampleeee";
            textBlock.HorizontalAlignment = HorizontalAlignment.Left;
            textBlock.VerticalAlignment = VerticalAlignment.Top;

            canvas.Children.Add(iRectangle);
            canvas.Children.Add(textBlock);

            grid.Children.Add(canvas);

            Window.Current.SizeChanged += CurrentOnSizeChanged;
        }

        /// <summary>
        /// Handles device rotation event.
        /// </summary>
        /// <param name="sender">the sender</param>
        /// <param name="windowSizeChangedEventArgs">event args</param>
        private void CurrentOnSizeChanged(object sender, WindowSizeChangedEventArgs windowSizeChangedEventArgs)
        {
            var bounds = Window.Current.Bounds;
            if (bounds.Width > bounds.Height)
            {
                iRectangle.Width = 300;
                iRectangle.Height = 100;
            }
            else
            {
                iRectangle.Width = 100;
                iRectangle.Height = 40;
            }

        }
    }
}
