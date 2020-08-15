using Diaspora.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                        profFrame.IsVisible = true;
                        loadAct.IsVisible = false;
                        loadAct.IsRunning = false;
                        Umail.Text = lastName.user_email;

                    }
                }
                else
                {
                    loadAct.IsVisible = false;
                    loadAct.IsRunning = false;
                    Navigation.PushAsync(new start());
                }
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
                if (rows > 0)
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
                    Navigation.PushAsync(new start());
                }
            }
        }
    }
}