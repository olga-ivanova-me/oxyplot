using System;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using OxyPlot.Windows;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace SimpleDemo.UWP
{
    public class UwpCustomControl : Control
    {
        /// <summary>
        /// The Grid PART constant.
        /// </summary>
        private const string PartGrid = "PART_Grid";

        private Rectangle iRectangle;
        private Grid iGrid;
        private Canvas iCanvas;

        /// <summary>
        /// Constructor
        /// </summary>
        public UwpCustomControl()
        {
            this.DefaultStyleKey = typeof(UwpCustomControl);
            Window.Current.SizeChanged += CurrentOnSizeChanged;
        }

        /// <summary>
        /// TODO remove
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="windowSizeChangedEventArgs"></param>
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

        /// <summary>
        /// From Control
        /// </summary>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            iGrid = GetTemplateChild(PartGrid) as Grid;
            if (iGrid == null)
            {
                return;
            }

            DrawRectangle();
        }

        /// <summary>
        /// Draws the rectangle with text.
        /// </summary>
        private void DrawRectangle()
        {
            iCanvas = new Canvas
            {
                IsHitTestVisible = false,
                Height = 200,
                Width = 350,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
            };
            
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

            iCanvas.Children.Add(iRectangle);
            iCanvas.Children.Add(textBlock);

            iGrid.Children.Add(iCanvas);

            iCanvas.UpdateLayout();
        }

        /// <summary>
        /// Handles size changed event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="sizeChangedEventArgs"></param>
        private void OnSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
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
