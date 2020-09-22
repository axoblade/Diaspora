using Diaspora.Model;
using Plugin.Geolocator;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
    public partial class phone : ContentPage
    {
        public phone()
        {
            InitializeComponent();
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        private async void mybtn_Clicked(object sender, EventArgs e)
        {
            //using (SQLiteConnection conn = new SQLiteConnection(App.Databaselocation))
            //{
            //    conn.CreateTable<Post>();
            //    var posts = conn.Table<Post>().ToList();
            //    int rows = posts.Count();
            //    if (rows > 0)
            //    {
            //        var lastName = posts.LastOrDefault();
            //        var LoggedIn = lastName.user_email;

            //        bool NoUser = string.IsNullOrEmpty(LoggedIn);

            //        if (NoUser == true)
            //        {
            //            await DisplayAlert("ALert", "No user selected", "Ok");
            //        }
            //        else
            //        {
            //            var Hist_username = lastName.user_email;
            bool isUsernameEmpty = string.IsNullOrEmpty(uphone.Text);

            if (isUsernameEmpty == true)
            {
                await DisplayAlert("Alert", "Phone number cannot be empty", "Ok");
            }
            else
            {
                try
                {
                    var network = Connectivity.NetworkAccess;
                    if (network != NetworkAccess.Internet)
                    {
                        await DisplayAlert("Alert", "You need an internet connection to continue", "Ok");
                    }
                    else
                    {
                        if (uphone.Text.Length < 10)
                        {
                            await DisplayAlert("Alert", "Are you sure this is a correct phone number?", "Ok");
                        }
                        else
                        {
                            mybtn.IsVisible = false;
                            activity_indicator.IsVisible = true;
                            activity_indicator.IsRunning = true;
                            activity_indicator.IsEnabled = true;
                            WebClient client = new WebClient();
                            Uri uri = new Uri("https://apis.paxol.cloud/start.php");
                            NameValueCollection parameters = new NameValueCollection();
                            parameters.Add("username", uphone.Text);
                            client.UploadValuesCompleted += Client_UploadValuesCompleted;
                            client.UploadValuesAsync(uri, parameters);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Unable to get location
                    await DisplayAlert("Error", "Reload the APP", "Ok");
                }
            }
            //        }
            //    }
            //    else
            //    {
            //        await Navigation.PushAsync(new start());
            //    }
            //}
        }
        private async void Client_UploadValuesCompleted(object sender, UploadValuesCompletedEventArgs e)
        {
            string r = Encoding.UTF8.GetString(e.Result);

            activity_indicator.IsVisible = false;
            activity_indicator.IsRunning = false;
            activity_indicator.IsEnabled = false;
            mybtn.IsVisible = true;

            if (r == "failed")
            {
                await DisplayAlert("Alert", r, "OK");
            }
            else
            {
                Post post = new Post()
                {
                    user_email = uphone.Text

                };
                using (SQLiteConnection conn = new SQLiteConnection(App.Databaselocation))
                {
                    conn.CreateTable<Post>();

                    int row = conn.Insert(post);

                    conn.Close();
                    if (row > 0)
                    {
                        await Navigation.PushAsync(new verify());
                    }
                }
            }

        }
    }
}