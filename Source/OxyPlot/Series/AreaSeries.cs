﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AreaSeries.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents an area series that fills the polygon defined by two sets of points or one set of points and a constant.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Series
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents an area series that fills the polygon defined by two sets of points or one set of points and a constant.
    /// </summary>
    public class AreaSeries : LineSeries
    {
        /// <summary>
        /// The second list of points.
        /// </summary>
        private readonly List<DataPoint> points2 = new List<DataPoint>();

        /// <summary>
        /// The secondary data points from the <see cref="P:ItemsSource" /> collection.
        /// </summary>
        private readonly List<DataPoint> itemsSourcePoints2 = new List<DataPoint>();

        /// <summary>
        /// The secondary data points from the <see cref="P:Points2" /> list.
        /// </summary>
        private List<DataPoint> actualPoints2;

        /// <summary>
        /// Initializes a new instance of the <see cref = "AreaSeries" /> class.
        /// </summary>
        public AreaSeries()
        {
            this.Reverse2 = true;
            this.Color2 = OxyColors.Automatic;
            this.Fill = OxyColors.Automatic;
        }

        /// <summary>
        /// Gets or sets a constant value for the area definition.
        /// This is used if DataFieldBase and BaselineValues are <c>null</c>.
        /// </summary>
        /// <value>The baseline.</value>
        /// <remarks><see cref="P:ConstantY2" /> is used if <see cref="P:ItemsSource" /> is set 
        /// and <see cref="P:DataFieldX2" /> or <see cref="P:DataFieldY2" /> are <c>null</c>, 
        /// or if <see cref="P:ItemsSource" /> is <c>null</c> and <see cref="P:Points2" /> is empty.</remarks>
        public double ConstantY2 { get; set; }

        /// <summary>
        /// Gets or sets the data field to use for the X-coordinates of the second data set.
        /// </summary>
        /// <remarks>This property is used if <see cref="P:ItemsSource" /> is set.</remarks>
        public string DataFieldX2 { get; set; }

        /// <summary>
        /// Gets or sets the data field to use for the Y-coordinates of the second data set.
        /// </summary>
        /// <remarks>This property is used if <see cref="P:ItemsSource" /> is set.</remarks>
        public string DataFieldY2 { get; set; }

        /// <summary>
        /// Gets or sets the color of the line for the second data set.
        /// </summary>
        /// <value>The color.</value>
        public OxyColor Color2 { get; set; }

        /// <summary>
        /// Gets the actual color of the line for the second data set.
        /// </summary>
        /// <value>The actual color.</value>
        public OxyColor ActualColor2
        {
            get
            {
                return this.Color2.GetActualColor(this.ActualColor);
            }
        }

        /// <summary>
        /// Gets or sets the fill color of the area.
        /// </summary>
        /// <value>The fill color.</value>
        public OxyColor Fill { get; set; }

        /// <summary>
        /// Gets the actual fill color of the area.
        /// </summary>
        /// <value>The actual fill color.</value>
        public OxyColor ActualFill
        {
            get
            {
                return this.Fill.GetActualColor(OxyColor.FromAColor(100, this.ActualColor));
            }
        }

        /// <summary>
        /// Gets the second list of points.
        /// </summary>
        /// <value>The second list of points.</value>
        /// <remarks>This property is not used if <see cref="P:ItemsSource" /> is set.</remarks>
        public List<DataPoint> Points2
        {
            get
            {
                return this.points2;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the second
        /// data collection should be reversed.
        /// </summary>
        /// <value><c>true</c> if the second data setshould be reversed; otherwise, <c>false</c>.</value>
        /// <remarks>The first dataset is not reversed, and normally
        /// the second dataset should be reversed to get a
        /// closed polygon.</remarks>
        public bool Reverse2 { get; set; }

        /// <summary>
        /// Gets the actual points of the second data set.
        /// </summary>
        /// <value>A list of data points.</value>
        protected List<DataPoint> ActualPoints2
        {
            get
            {
                return this.ItemsSource != null ? this.itemsSourcePoints2 : this.actualPoints2;
            }
        }

        /// <summary>
        /// Gets the nearest point.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="interpolate">interpolate if set to <c>true</c> .</param>
        /// <returns>A TrackerHitResult for the current hit.</returns>
        public override TrackerHitResult GetNearestPoint(ScreenPoint point, bool interpolate)
        {
            TrackerHitResult result1, result2;
            if (interpolate && this.CanTrackerInterpolatePoints)
            {
                result1 = this.GetNearestInterpolatedPointInternal(this.ActualPoints, point);
                result2 = this.GetNearestInterpolatedPointInternal(this.ActualPoints2, point);
            }
            else
            {
                result1 = this.GetNearestPointInternal(this.ActualPoints, point);
                result2 = this.GetNearestPointInternal(this.ActualPoints2, point);
            }

            TrackerHitResult result;
            if (result1 != null && result2 != null)
            {
                double dist1 = result1.Position.DistanceTo(point);
                double dist2 = result2.Position.DistanceTo(point);
                result = dist1 < dist2 ? result1 : result2;
            }
            else
            {
                result = result1 ?? result2;
            }

            if (result != null)
            {
                result.Text = StringHelper.Format(
                    this.ActualCulture,
                    this.TrackerFormatString,
                    result.Item,
                    this.Title,
                    this.XAxis.Title ?? XYAxisSeries.DefaultXAxisTitle,
                    this.XAxis.GetValue(result.DataPoint.X),
                    this.YAxis.Title ?? XYAxisSeries.DefaultYAxisTitle,
                    this.YAxis.GetValue(result.DataPoint.Y));
            }

            return result;
        }

        /// <summary>
        /// Renders the series on the specified rendering context.
        /// </summary>
        /// <param name="rc">The rendering context.</param>
        public override void Render(IRenderContext rc)
        {
            var actualPoints = this.ActualPoints;
            var actualPoints2 = this.ActualPoints2;
            if (actualPoints.Count == 0)
            {
                return;
            }

            this.VerifyAxes();

            double minDistSquared = this.MinimumSegmentLength * this.MinimumSegmentLength;

            var clippingRect = this.GetClippingRect();
            rc.SetClip(clippingRect);

            // Manage NaN's
            var chunkedListsOfPoints = this.Split(actualPoints, p => double.IsNaN(p.Y));
            var chunkedListsOfPoints2 = this.Split(actualPoints2, p => double.IsNaN(p.Y));

            for (int chunkIndex = 0; chunkIndex < chunkedListsOfPoints.Count(); chunkIndex++)
            {
                var chunkActualPoints = chunkedListsOfPoints.ElementAt(chunkIndex).ToList();

                // Transform all points to screen coordinates
                int n0 = chunkActualPoints.Count;
                IList<ScreenPoint> pts0 = new ScreenPoint[n0];
                for (int i = 0; i < n0; i++)
                {
                    pts0[i] = this.XAxis.Transform(chunkActualPoints[i].X, chunkActualPoints[i].Y, this.YAxis);
                }

                if (this.Smooth)
                {
                    var rpts0 = ScreenPointHelper.ResamplePoints(pts0, this.MinimumSegmentLength);
                    pts0 = CanonicalSplineHelper.CreateSpline(rpts0, 0.5, null, false, 0.25);
                }

                var dashArray = this.ActualDashArray;

                // draw the clipped lines
                rc.DrawClippedLine(
                    clippingRect,
                    pts0,
                    minDistSquared,
                    this.GetSelectableColor(this.ActualColor),
                    this.StrokeThickness,
                    dashArray,
                    this.LineJoin,
                    false);
            }

            for (int chunkIndex = 0; chunkIndex < chunkedListsOfPoints2.Count(); chunkIndex++)
            {
                var chunkActualPoints2 = chunkedListsOfPoints2.ElementAt(chunkIndex).ToList();

                // Transform all points to screen coordinates
                int n1 = chunkActualPoints2.Count;
                IList<ScreenPoint> pts1 = new ScreenPoint[n1];
                for (int i = 0; i < n1; i++)
                {
                    int j = this.Reverse2 ? n1 - 1 - i : i;
                    pts1[j] = this.XAxis.Transform(chunkActualPoints2[i].X, chunkActualPoints2[i].Y, this.YAxis);
                }

                if (this.Smooth)
                {
                    var rpts1 = ScreenPointHelper.ResamplePoints(pts1, this.MinimumSegmentLength);
                    pts1 = CanonicalSplineHelper.CreateSpline(rpts1, 0.5, null, false, 0.25);
                }

                var dashArray = this.ActualDashArray;

                // draw the clipped lines
                rc.DrawClippedLine(
                    clippingRect,
                    pts1,
                    minDistSquared,
                    this.GetSelectableColor(this.ActualColor2),
                    this.StrokeThickness,
                    dashArray,
                    this.LineJoin,
                    false);
            }

            if (chunkedListsOfPoints.Count() != chunkedListsOfPoints2.Count())
            {
                rc.ResetClip();
                return;
            }

            // Draw the fill
            for (int chunkIndex = 0; chunkIndex < chunkedListsOfPoints.Count(); chunkIndex++)
            {
                var chunkActualPoints = chunkedListsOfPoints.ElementAt(chunkIndex).ToList();
                var chunkActualPoints2 = chunkedListsOfPoints2.ElementAt(chunkIndex).ToList();

                // Transform all points to screen coordinates
                int n0 = chunkActualPoints.Count;
                IList<ScreenPoint> pts0 = new ScreenPoint[n0];
                for (int i = 0; i < n0; i++)
                {
                    pts0[i] = this.XAxis.Transform(chunkActualPoints[i].X, chunkActualPoints[i].Y, this.YAxis);
                }

                int n1 = chunkActualPoints2.Count;
                IList<ScreenPoint> pts1 = new ScreenPoint[n1];
                for (int i = 0; i < n1; i++)
                {
                    int j = this.Reverse2 ? n1 - 1 - i : i;
                    pts1[j] = this.XAxis.Transform(chunkActualPoints2[i].X, chunkActualPoints2[i].Y, this.YAxis);
                }

                if (this.Smooth)
                {
                    var rpts0 = ScreenPointHelper.ResamplePoints(pts0, this.MinimumSegmentLength);
                    var rpts1 = ScreenPointHelper.ResamplePoints(pts1, this.MinimumSegmentLength);

                    pts0 = CanonicalSplineHelper.CreateSpline(rpts0, 0.5, null, false, 0.25);
                    pts1 = CanonicalSplineHelper.CreateSpline(rpts1, 0.5, null, false, 0.25);
                }

                var dashArray = this.ActualDashArray;

                // combine the two lines and draw the clipped area
                var pts = new List<ScreenPoint>();
                pts.AddRange(pts1);
                pts.AddRange(pts0);

                // pts = SutherlandHodgmanClipping.ClipPolygon(clippingRect, pts);
                rc.DrawClippedPolygon(clippingRect, pts, minDistSquared, this.GetSelectableFillColor(this.ActualFill), OxyColors.Undefined);

                var markerSizes = new[] { this.MarkerSize };

                // draw the markers on top
                rc.DrawMarkers(
                    clippingRect,
                    pts0,
                    this.MarkerType,
                    null,
                    markerSizes,
                    this.MarkerFill,
                    this.MarkerStroke,
                    this.MarkerStrokeThickness,
                    1);
                rc.DrawMarkers(
                    clippingRect,
                    pts1,
                    this.MarkerType,
                    null,
                    markerSizes,
                    this.MarkerFill,
                    this.MarkerStroke,
                    this.MarkerStrokeThickness,
                    1);
            }

            rc.ResetClip();
        }

        /// <summary>
        /// Renders the legend symbol for the line series on the
        /// specified rendering context.
        /// </summary>
        /// <param name="rc">The rendering context.</param>
        /// <param name="legendBox">The bounding rectangle of the legend box.</param>
        public override void RenderLegend(IRenderContext rc, OxyRect legendBox)
        {
            double y0 = (legendBox.Top * 0.2) + (legendBox.Bottom * 0.8);
            double y1 = (legendBox.Top * 0.4) + (legendBox.Bottom * 0.6);
            double y2 = (legendBox.Top * 0.8) + (legendBox.Bottom * 0.2);

            var pts0 = new[] { new ScreenPoint(legendBox.Left, y0), new ScreenPoint(legendBox.Right, y0) };
            var pts1 = new[] { new ScreenPoint(legendBox.Right, y2), new ScreenPoint(legendBox.Left, y1) };
            var pts = new List<ScreenPoint>();
            pts.AddRange(pts0);
            pts.AddRange(pts1);
            rc.DrawLine(pts0, this.GetSelectableColor(this.ActualColor), this.StrokeThickness, this.ActualLineStyle.GetDashArray());
            rc.DrawLine(pts1, this.GetSelectableColor(this.ActualColor2), this.StrokeThickness, this.ActualLineStyle.GetDashArray());
            rc.DrawPolygon(pts, this.GetSelectableFillColor(this.ActualFill), OxyColors.Undefined);
        }

        /// <summary>
        /// The update data.
        /// </summary>
        protected internal override void UpdateData()
        {
            base.UpdateData();

            if (this.ItemsSource == null)
            {
                if (this.points2.Count > 0)
                {
                    this.actualPoints2 = this.points2;
                } else { 
                    this.actualPoints2 = this.GetConstantPoints2().ToList();
                }

                return;
            }

            this.itemsSourcePoints2.Clear();

            // TODO: make it consistent with DataPointSeries.UpdateItemsSourcePoints
            // Using reflection on DataFieldX2 and DataFieldY2
            if (this.DataFieldX2 != null && this.DataFieldY2 != null)
            {
                ReflectionExtensions.AddRange(this.itemsSourcePoints2, this.ItemsSource, this.DataFieldX2,
                    this.DataFieldY2);
            }
            else
            {
                this.itemsSourcePoints2.AddRange(this.GetConstantPoints2());
            }
        }

        /// <summary>
        /// Updates the maximum and minimum values of the series.
        /// </summary>
        protected internal override void UpdateMaxMin()
        {
            base.UpdateMaxMin();
            this.InternalUpdateMaxMin(this.ActualPoints2);
        }

        /// <summary>
        /// Gets the points when <see cref="P:ConstantY2" /> is used.
        /// </summary>
        /// <returns>A sequence of <see cref="T:DataPoint"/>.</returns>
        private IEnumerable<DataPoint> GetConstantPoints2()
        {
            var list  = new List<DataPoint>();
            var actualPoints = this.ActualPoints;
            if (!double.IsNaN(this.ConstantY2) && actualPoints.Count > 0)
            {
                // Use ConstantY2
                var x0 = actualPoints[0].X;
                var x1 = actualPoints[actualPoints.Count - 1].X;
                list.Add(new DataPoint(x0, this.ConstantY2));
                list.Add(new DataPoint(x1, this.ConstantY2));
            }
            return list;
        }

        /// <summary>
        /// Split an IEnumerable<typeparamref name="T"/> into chunks (sub-lists), based on a split condition
        /// (input items with splitCondition == true will not be included in output)
        /// </summary>
        /// <typeparam name="T">The type of the input list item</typeparam>
        /// <param name="source">The input list</param>
        /// <param name="splitCondition">The split condition</param>
        /// <returns>A collection of a collection of <typeparamref name="T"/> items</returns>
        private IEnumerable<IEnumerable<T>> Split<T>(IEnumerable<T> source, Func<T, bool> splitCondition)
        {
            var list = new List<IEnumerable<T>>();
            source = source.SkipWhile(splitCondition);
            while (source.Any())
            {
                list.Add(source.TakeWhile(x => !splitCondition(x)));
                source = source.SkipWhile(x => !splitCondition(x)).SkipWhile(splitCondition);
            }

            return list;
        }
    }
}