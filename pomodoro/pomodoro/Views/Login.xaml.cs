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
    public partial class Login : ContentPage
    {
        public Login()
        {
            InitializeComponent();
        }

        private void btnLoginPopup_Clicked(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PushAsync(new LoginPopup());
        }
      

        private async void btnRegisterPopup_Clicked(object sender, EventArgs e)
        {
            
                await PopupNavigation.Instance.PushAsync(new RegisterPopup());

                

        }

    }
}