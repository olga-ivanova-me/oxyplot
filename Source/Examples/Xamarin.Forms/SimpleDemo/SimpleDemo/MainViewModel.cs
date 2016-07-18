using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace SimpleDemo
{
    public class MainViewModel
    {
        public MainViewModel()
        {
            var model = new PlotModel
            {
                Title = "Defined by Items",
                Subtitle = "The items are added to the `Items` property.",
                IsLegendVisible = false,
            };

            var s1 = new ColumnSeries
            {
                Title = "Series 1",
                LabelPlacement = LabelPlacement.Inside,
                LabelFormatString = "text"
            };

            s1.Items.AddRange(new[] { new ColumnItem { Value = 25 }, new ColumnItem { Value = 137 } });

            var s2 = new ColumnSeries
            {
                Title = "Series 2",
                LabelPlacement = LabelPlacement.Inside,
                LabelFormatString = "text"
            };
            s2.Items.AddRange(new[] { new ColumnItem { Value = 52 }, new ColumnItem { Value = 317 } });

            model.Series.Add(s1);
            model.Series.Add(s2);

            model.Axes.Add(new CategoryAxis { Position = AxisPosition.Bottom, IsPanEnabled = false, IsZoomEnabled = false, });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, MinimumPadding = 0, IsPanEnabled = false, IsZoomEnabled = false, });

            this.Model = model;
        }

        public PlotModel Model { get; private set; }

        public string Sample => "SAMPPLE HEader";

        public IList<string> List
        {
            get
            {
                var list = new List<string>();
                list.Add("test1");
                list.Add("test2");
                list.Add("test3");
                list.Add("test4");
                list.Add("test5");

                return list;
            }
        }
    }
}
