using System.ComponentModel;
using Windows.UI.Xaml.Controls;
using SimpleDemo;
using SimpleDemo.UWP;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;


[assembly: ExportRenderer(typeof(CustomLabel), typeof(CustomLabelRenderer))]

namespace SimpleDemo.UWP
{
    public class CustomLabelRenderer : ViewRenderer<Label, Control>
    {
        /// <summary>
        /// From LAbelRenderer
        /// </summary>
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                SetNativeControl(new UwpCustomControl());
            }
        }

        /// <summary>
        /// From LabelRenderer
        /// </summary>
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
        }
    }
}
