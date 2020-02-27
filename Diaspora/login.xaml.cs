using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Diaspora
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class login : ContentPage
    {
        public login()
        {
            InitializeComponent();
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        private void mybtn_Clicked(object sender, EventArgs e)
        {
            if (uname.Text != "")
            {
                WebClient client = new WebClient();
                Uri uri = new Uri("http://www.akyinvestmentsltd.com/DiasporaApi/login.php");
                NameValueCollection parameters = new NameValueCollection();
                parameters.Add("username", uname.Text);
                parameters.Add("password", pswd.Text);
                client.UploadValuesCompleted += Client_UploadValuesCompleted;
                client.UploadValuesAsync(uri, parameters);
            }
            else
            {
                DisplayAlert("Alert", "Username or Password Cannot be empty", "Ok");
            }
        }

        private void Client_UploadValuesCompleted(object sender, UploadValuesCompletedEventArgs e)
        {
            var r = Encoding.UTF8.GetString(e.Result);
            if(r == "Login failed")
            {
                DisplayAlert("Alert", r, "OK");
                uname.Text = "";
                pswd.Text = "";
            }
            else
            {
                //login successful
            }
        }
    }
}