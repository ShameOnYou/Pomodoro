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
    public partial class TaskPopup
    {
        public TaskPopup()
        {
            InitializeComponent();
        }

        private void btnSubmit_Clicked(object sender, EventArgs e)
        {
            
            var task = new Data.PomTask(entryName.Text, 0, Convert.ToInt32(entryNumber.Text));
            Data.Databank.addTask(task);
            Console.WriteLine(Data.Databank.Tasks.Count);
            UpdateListView(MainPage.lstView);
            PopupNavigation.Instance.PopAsync(true);
           
        }

        void UpdateListView(ListView listView)
        {

            var itemsSource = listView.ItemsSource;
            listView.ItemsSource = null;
            listView.ItemsSource = Data.Databank.Tasks;
        }
    }
}