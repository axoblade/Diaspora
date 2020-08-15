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
    public partial class start : ContentPage
    {
        public start()
        {
            InitializeComponent();
        }
        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        private async void regBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Register());
        }

        private  async void mybtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new login());
        }
    }
}