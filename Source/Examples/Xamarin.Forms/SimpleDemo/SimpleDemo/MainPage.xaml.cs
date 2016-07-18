using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace SimpleDemo
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            ListView.ItemSelected += ListViewBase_OnItemClick;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ListViewBase_OnItemClick(object sender, SelectedItemChangedEventArgs e)
        {
            await Navigation.PushAsync(new SecondPage());
        }
    }
}
