using Diaspora.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
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

        private async void mybtn_Clicked(object sender, EventArgs e)
        {
            activity.IsEnabled = true;
            activity.IsRunning = true;
            activity.IsVisible = true;

            var network = Connectivity.NetworkAccess;
            if(network == NetworkAccess.Unknown)
            {
                activity.IsEnabled = false;
                activity.IsVisible = false;
                activity.IsRunning = false;
                loginView.IsVisible = true;
                await DisplayAlert("Alert", "You need an internet connection to continue", "Ok");
            }
            if (network == NetworkAccess.Internet)
            {
                loginView.IsVisible = false;
                bool isUsernameEmpty = string.IsNullOrEmpty(uname.Text);
                bool isPswdEmpty = string.IsNullOrEmpty(pswd.Text);

                if (isUsernameEmpty || isPswdEmpty == true)
                {
                    activity.IsEnabled = false;
                    activity.IsVisible = false;
                    activity.IsRunning = false;
                    loginView.IsVisible = true;
                    await DisplayAlert("Alert", "Username or Password Cannot be empty", "Ok");
                }
                else
                {
                    WebClient client = new WebClient();
                    Uri uri = new Uri("https://apis.paxol.cloud/login.php");
                    NameValueCollection parameters = new NameValueCollection();
                    parameters.Add("username", uname.Text);
                    parameters.Add("password", pswd.Text);
                    client.UploadValuesCompleted += Client_UploadValuesCompleted;
                    client.UploadValuesAsync(uri, parameters);
                }
            }
            else
            {
                activity.IsEnabled = false;
                activity.IsVisible = false;
                activity.IsRunning = false;
                loginView.IsVisible = true;
                await DisplayAlert("Alert", "You need an internet connection to continue", "Ok");
            }
        }

        private async void Client_UploadValuesCompleted(object sender, UploadValuesCompletedEventArgs e)
        {
            var r = Encoding.UTF8.GetString(e.Result);    //.UTF8.GetString(e.Result);

            var axo = r.ToString();

            if(axo == "Success")
            {
                Post post = new Post()
                {
                    user_email = uname.Text

                };
                using (SQLiteConnection conn = new SQLiteConnection(App.Databaselocation))
                {
                    conn.CreateTable<Post>();

                    int row = conn.Insert(post);

                    conn.Close();

                    if (row > 0)
                    {
                        activity.IsEnabled = false;
                        activity.IsVisible = false;
                        activity.IsRunning = false;
                        await Navigation.PushAsync(new main());
                    }
                    else
                    {
                        activity.IsEnabled = false;
                        activity.IsVisible = false;
                        activity.IsRunning = false;
                        await DisplayAlert("Alert", "There is a server Error please try again later", "Ok");
                        uname.Text = null;
                        pswd.Text = null;
                        loginView.IsVisible = true;
                    }
                }
            }
            if (axo == "contact")
            {
                Post post = new Post()
                {
                    user_email = uname.Text

                };
                using (SQLiteConnection conn = new SQLiteConnection(App.Databaselocation))
                {
                    conn.CreateTable<Post>();

                    int row = conn.Insert(post);

                    conn.Close();

                    if (row > 0)
                    {
                        activity.IsEnabled = false;
                        activity.IsVisible = false;
                        activity.IsRunning = false;
                        await Navigation.PushAsync(new phone());
                    }
                    else
                    {
                        activity.IsEnabled = false;
                        activity.IsVisible = false;
                        activity.IsRunning = false;
                        await DisplayAlert("Alert", "There is a server Error please try again later", "Ok");
                        uname.Text = null;
                        pswd.Text = null;
                        loginView.IsVisible = true;
                    }
                }
            }
            else
            {
                activity.IsEnabled = false;
                activity.IsVisible = false;
                activity.IsRunning = false;
                await DisplayAlert(null, axo, "OK");
                uname.Text = null;
                pswd.Text = null;
                loginView.IsVisible = true;
            }
            
        }
    }
}