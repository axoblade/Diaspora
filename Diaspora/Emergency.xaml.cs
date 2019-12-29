using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Diaspora
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Emergency : ContentPage
    {
        public Emergency()
        {
            InitializeComponent();
        }

        private async void prof_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Profile());
        }
    }
}