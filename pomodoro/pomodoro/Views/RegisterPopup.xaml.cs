using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace pomodoro
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterPopup
    {
        public RegisterPopup()
        {
            InitializeComponent();
        }

        private async void btnRegister_Clicked(object sender, EventArgs e)
        {
           await Data.Databank.Register(entryEmail.Text, entryUsername.Text, entryPassword.Text);
           await PopupNavigation.Instance.PopAsync(true);
        }
    }
}