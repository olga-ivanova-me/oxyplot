// --------------------------------------------------------------------------------------------------------------------
// <copyright file="App.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace SimpleDemo
{
    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;
    using OxyPlot.Xamarin.Forms;

    using Xamarin.Forms;

    /// <summary>
    /// Represents a simple demo app (portable library).
    /// </summary>
    public class App : Application
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="App"/> class.
        /// </summary>
        public App()
        {
            MainPage = new NavigationPage(new MainPage());
        }
    }
}
