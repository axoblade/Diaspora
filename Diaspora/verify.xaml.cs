using Diaspora.Model;
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
    public partial class verify : ContentPage
    {
        public verify()
        {
            InitializeComponent();
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        private async void mybtn_Clicked(object sender, EventArgs e)
        {
            using (SQLiteConnection conn = new SQLiteConnection(App.Databaselocation))
            {
                conn.CreateTable<Post>();
                var posts = conn.Table<Post>().ToList();
                int rows = posts.Count();
                if (rows > 0)
                {
                    var lastName = posts.LastOrDefault();
                    var LoggedIn = lastName.user_email;

                    bool NoUser = string.IsNullOrEmpty(LoggedIn);

                    if (NoUser == true)
                    {
                        await DisplayAlert("ALert", "No user selected", "Ok");
                    }
                    else
                    {
                        var Hist_username = lastName.user_email;
                        try
                        {
                            var network = Connectivity.NetworkAccess;
                            if (network != NetworkAccess.Internet)
                            {
                                await DisplayAlert("Alert", "You need an internet connection to continue", "Ok");
                            }
                            else
                            {
                                mybtn.IsVisible = false;
                                activity_indicator.IsVisible = true;
                                activity_indicator.IsRunning = true;
                                activity_indicator.IsEnabled = true;
                                WebClient client = new WebClient();
                                Uri uri = new Uri("https://apis.paxol.cloud/verify.php");
                                NameValueCollection parameters = new NameValueCollection();
                                parameters.Add("username", Hist_username);
                                parameters.Add("code", Vcode.Text);
                                client.UploadValuesCompleted += Client_UploadValuesCompleted;
                                client.UploadValuesAsync(uri, parameters);
                            }
                        }
                        catch (Exception ex)
                        {
                            // Unable to get location
                            await DisplayAlert("Error", "Reload the APP", "Ok");
                        }
                    }
                }
                else
                {
                    await Navigation.PushAsync(new start());
                }
            }
        }
        private async void Client_UploadValuesCompleted(object sender, UploadValuesCompletedEventArgs e)
        {
            activity_indicator.IsVisible = false;
            
            activity_indicator.IsRunning = false;
            
            activity_indicator.IsEnabled = false;
            
            mybtn.IsVisible = true;

            string r = Encoding.UTF8.GetString(e.Result);
            if (r == "success")
            {
                await Navigation.PushAsync(new main());
            }
            else
            {
                await DisplayAlert("Alert", r, "OK");
            }

        }
        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            using (SQLiteConnection conn = new SQLiteConnection(App.Databaselocation))
            {
                conn.DeleteAll<Model.Post>();
                var posts = conn.Table<Post>();
                int rows = posts.Count();
                if (rows > 0)
                {
                    DisplayAlert("Error", "Cannot logout", "Ok");
                }
                else
                {
                    Navigation.PushAsync(new phone());
                }
            }
        }
    }
}