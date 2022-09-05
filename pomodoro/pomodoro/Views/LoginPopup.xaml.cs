using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace pomodoro
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPopup
    {
        public LoginPopup()
        {
            InitializeComponent();
        }
        bool success;
        private async void btnLogin_Clicked(object sender, EventArgs e)
        {
            success = Task.Run(() => Data.Databank.Login(entryEmail.Text, entryPassword.Text)).Result;
           
            if(success == true)
            {
                
                await PopupNavigation.Instance.PopAsync(true);
                
            }
            else
            {
              
                await PopupNavigation.Instance.PopAsync(true);
            }
        }
        protected async override void OnDisappearing()
        {
            base.OnDisappearing();

            if (success)
            {
                await Navigation.PushAsync(new NavigationPage(new MainPage()));
            }

        }
    }
}