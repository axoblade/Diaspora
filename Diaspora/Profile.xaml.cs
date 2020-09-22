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
    public partial class Profile : ContentPage
    {
        public Profile()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            profFrame.IsVisible = false;
            loadAct.IsVisible = true;
            loadAct.IsRunning = true;
            using (SQLiteConnection conn = new SQLiteConnection(App.Databaselocation))
            {
                conn.CreateTable<Post>();
                var posts = conn.Table<Post>().ToList();
                int rows = posts.Count();
                if(rows > 0)
                {
                    var lastName = posts.LastOrDefault();
                    var LoggedIn = lastName.user_email;

                    bool NoUser = string.IsNullOrEmpty(LoggedIn);

                    if (NoUser == true)
                    {
                        DisplayAlert("ALert", "No user selected", "Ok");
                    }
                    else
                    {
                        Umail.Text = lastName.user_email;
                        var nemsis = lastName.user_email;
                        try
                        {
                            var network = Connectivity.NetworkAccess;
                            if (network != NetworkAccess.Internet)
                            {
                                DisplayAlert("Alert", "You need an internet connection to continue", "Ok");
                            }
                            else
                            {
                                    WebClient client = new WebClient();
                                    Uri uri = new Uri("https://apis.paxol.cloud/check.php");
                                    NameValueCollection parameters = new NameValueCollection();
                                    parameters.Add("username", nemsis);
                                    client.UploadValuesCompleted += Client_UploadValuesCompleted;
                                    client.UploadValuesAsync(uri, parameters);

                            }
                        }
                        catch(Exception ex)
                        {
                            // Unable to get location
                            DisplayAlert("Error", "Reload the APP", "Ok");
                        }

                    }
                }
                else
                {
                    loadAct.IsVisible = false;
                    loadAct.IsRunning = false;
                    Navigation.PushAsync(new phone());
                }
            }
        }

        private async void Client_UploadValuesCompleted(object sender, UploadValuesCompletedEventArgs e)
        {
            string r = Encoding.UTF8.GetString(e.Result);

            if(r == "failed")
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.Databaselocation))
                {
                    conn.DeleteAll<Post>();
                    var posts = conn.Table<Post>();
                    int rows = posts.Count();
                    if (rows < 0)
                    {
                        profFrame.IsVisible = true;
                        loadAct.IsVisible = false;
                        loadAct.IsRunning = false;
                        await DisplayAlert("Error", "Cannot logout", "Ok");
                    }
                    else
                    {
                        loadAct.IsVisible = false;
                        loadAct.IsRunning = false;
                        await Navigation.PushAsync(new phone());
                    }
                }
            }
            else
            {
                profFrame.IsVisible = true;
                loadAct.IsVisible = false;
                loadAct.IsRunning = false;
            }

        }
        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            loadAct.IsVisible = true;
            loadAct.IsRunning = true;
            using (SQLiteConnection conn = new SQLiteConnection(App.Databaselocation))
            {
                conn.DeleteAll<Post>();
                var posts = conn.Table<Post>();
                int rows = posts.Count();
                if (rows < 0)
                {
                    profFrame.IsVisible = true;
                    loadAct.IsVisible = false;
                    loadAct.IsRunning = false;
                    DisplayAlert("Error", "Cannot logout", "Ok");
                }
                else
                {
                    loadAct.IsVisible = false;
                    loadAct.IsRunning = false;
                    Navigation.PushAsync(new phone());
                }
            }
        }
    }
}