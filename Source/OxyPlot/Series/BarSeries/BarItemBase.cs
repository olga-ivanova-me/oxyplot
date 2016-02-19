// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BarItemBase.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents an item used in the BarSeriesBase.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Series
{
    /// <summary>
    /// Represents an item used in the BarSeriesBase.
    /// </summary>
    public abstract class BarItemBase : CategorizedItem, ICodeGenerating
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BarItemBase" /> class. Initializes a new instance of the <see cref="BarItem" /> class.
        /// </summary>
        protected BarItemBase()
        {
            // Label = null;
            this.Value = double.NaN;
            this.Color = OxyColors.Automatic;
			this.TextColor = OxyColors.Undefined;
            this.AlternativeTextColor = OxyColors.Undefined;
        }

        /// <summary>
        /// Gets or sets the color of the item.
        /// </summary>
        /// <remarks>If the color is not specified (default), the color of the series will be used.</remarks>
        public OxyColor Color { get; set; }

        /// <summary>
        /// Gets or sets the value of shadow radius.
        /// </summary>
        /// <remarks>If the radius is not specified (default), the shadow radius of the series will be used.</remarks>
        public float ShadowRadius { get; set; }

		/// <summary>
        /// Gets or sets the text color of the item.
        /// </summary>
        /// <remarks>If the color is not specified (default), the color of the series will be used.</remarks>
        public OxyColor TextColor { get; set; }

        /// <summary>
        /// Gets or sets the text color of the bar labels when their placement differ from the 
        /// set property <see cref="P:LabelPlacement"/>. 
        /// (e.g. when bar label should be placed inside the bar but there are no place for label. 
        /// So, it will be placed outside with this color.)
        /// </summary>
        /// <remarks>If the color is not specified (default), the color of the series will be used.</remarks>
        public OxyColor AlternativeTextColor { get; set; }

        /// <summary>
        /// Gets or sets the value of the item.
        /// </summary>
        public double Value { get; set; }

        /// <summary>
        /// Returns c# code that generates this instance.
        /// </summary>
        /// <returns>C# code.</returns>
        public virtual string ToCode()
        {
            if (!this.Color.IsUndefined())
            {
                return CodeGenerator.FormatConstructor(
                    this.GetType(), "{0},{1},{2}", this.Value, this.CategoryIndex, this.Color.ToCode());
            }

            if (this.CategoryIndex != -1)
            {
                return CodeGenerator.FormatConstructor(this.GetType(), "{0},{1}", this.Value, this.CategoryIndex);
            }

            return CodeGenerator.FormatConstructor(this.GetType(), "{0}", this.Value);
        }
    }
}