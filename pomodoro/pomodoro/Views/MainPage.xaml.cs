using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace pomodoro
{


    public partial class MainPage : ContentPage
    {
        private string current = "pomodoro";
        private int breakTimes = 0;

        bool cancelTimer = false;
        bool timerRunning = false;
        bool isBreak = false;
        private int _countSeconds;

        public MainPage()
        {
            InitializeComponent();
            lblUsername.Text = "Hi " + Data.Databank.userLoggedIn;
            var task = Task.Run(() => Data.Databank.initializeDataBank()).Result;
            ListViewTasks.ItemsSource = Data.Databank.Tasks;
            UpdateListView(ListViewTasks);
            if (Data.Databank.Tasks.Any())
            {
                int maxPom = Data.Databank.Tasks.Where(x => x.active = true).ToList()[0].MaxPom;
                int currPom = Data.Databank.Tasks.Where(x => x.active = true).ToList()[0].CurrPom;
                lblTaskName.Text = Data.Databank.Tasks.Where(x => x.active = true).ToList()[0].TaskName;
                lblTasksCompleted.Text = currPom.ToString() + " / " + maxPom.ToString();
                btnStartTimer.IsEnabled = false;
            }
            
            
        }

        public static ListView lstView;
        private void btnAddTask_Clicked(object sender, EventArgs e)
        {
            lstView = ListViewTasks;
            PopupNavigation.Instance.PushAsync(new TaskPopup());

        }

     
        private void ListViewTasks_ItemTapped(object sender, ItemTappedEventArgs e)
        {
             var dataItem = e.Item as Data.PomTask;
            dataItem.active = true;
            btnStartTimer.IsEnabled = true;
            Data.Databank.Tasks.Where(x => x.taskId != dataItem.taskId).ToList().ForEach(x => x.active = false);
            Data.Databank.updateTask(dataItem);
            UpdateListView(ListViewTasks);
            
            lblTimer.Text = "25 : 00";
            btnStartTimer.Text = "Start";
            current = "pomodoro";
            breakTimes = 0;
            if (timerRunning)
            {
                cancelTimer = true;
            }
            isBreak = false;
            btnPomodoro.BackgroundColor = Color.FromRgba(55, 55, 55, 55);
            btnShortBreak.BackgroundColor = Color.Transparent;
            btnLongBreak.BackgroundColor = Color.Transparent;

        }

        void UpdateCurrentTask()
        {
            if (Data.Databank.Tasks.Any())
            {
                var activeTask = new Data.PomTask();

                try
                {
                    activeTask = Data.Databank.Tasks.Where(x => x.active == true).ToList()[0];
                }
                catch(Exception e)
                {
                    Data.Databank.Tasks.Where(x => x.ValidTask == true).ToList()[0].active = true;
                    activeTask = Data.Databank.Tasks.Where(x => x.active == true).ToList()[0];

                }

                int maxPom = Convert.ToInt32(activeTask.MaxPom);
                int currPom = Convert.ToInt32(activeTask.CurrPom);
                lblTaskName.Text = activeTask.TaskName;
                lblTasksCompleted.Text = currPom.ToString() + " / " + maxPom.ToString();
            }
          

            
    }

        public void UpdateListView(ListView listView)
        {

            var itemSource = listView.ItemsSource;
            listView.ItemsSource = null;
            listView.ItemsSource = itemSource;

            UpdateCurrentTask();
        }

        private void btnStartTimer_Clicked(object sender, EventArgs e)
        {

            if (timerRunning)
            {
                if (isBreak && breakTimes != 2)
                {
                    btnPomodoro.BackgroundColor = Color.Transparent;
                    btnShortBreak.BackgroundColor = Color.FromRgba(55, 55, 55, 55);
                    btnLongBreak.BackgroundColor = Color.Transparent;
                    lblTimer.Text = "05 : 00";
                }
                else if (isBreak && breakTimes == 2)
                {
                    btnPomodoro.BackgroundColor = Color.Transparent;
                    btnShortBreak.BackgroundColor = Color.Transparent;
                    btnLongBreak.BackgroundColor = Color.FromRgba(55, 55, 55, 55);
                    lblTimer.Text = "10 : 00";
                }
                else if (isBreak == false)
                {
                    btnPomodoro.BackgroundColor = Color.FromRgba(55, 55, 55, 55);
                    btnShortBreak.BackgroundColor = Color.Transparent;
                    btnLongBreak.BackgroundColor = Color.Transparent;
                    lblTimer.Text = "25 : 00";
                }

                timerRunning = false;
                btnStartTimer.Text = "Start";
                cancelTimer = true;
            }else if (!timerRunning)
            {

                if (isBreak && breakTimes != 2)
                {
                    
                    breakTimes++;
                    current = "shortBreak";
                    isBreak = false;
                }
                else if (isBreak && breakTimes == 2)
                {
                   
                    breakTimes = 0;
                    current = "longBreak";
                    isBreak = false;
                }
                else if (isBreak == false)
                {
                    
                    current = "pomodoro";
                    isBreak = true;

                }

                switch (current)
                {
                    case "pomodoro":
                        _countSeconds = 10;
                        break;
                    case "shortBreak":
                        _countSeconds = 5;
                        break;
                    case "longBreak":
                        _countSeconds = 7;
                        break;
                }

                timerRunning = true;
                btnStartTimer.Text = "Stop";
                startTimer();
            }
              
        }

        void startTimer()
        {
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {

                if (cancelTimer)
                {
                    cancelTimer = false;
                    timerRunning = false;
                    return false;
                }

                _countSeconds--;

                double secToMin = Convert.ToDouble(_countSeconds) / 60;
                int mins = Convert.ToInt32(Math.Floor(secToMin));
                string customMin = mins.ToString();
                if (mins < 10)
                {
                    customMin = "0" + mins.ToString();
                }
                int secs = Convert.ToInt32(Math.Round((secToMin - Convert.ToDouble(mins)) * 60));
                string customSec = secs.ToString();
                if (secs < 10)
                {
                    customSec = "0" + secs.ToString();
                }

                lblTimer.Text = customMin + " : " + customSec;
                if (!Convert.ToBoolean(_countSeconds))
                {
                    TimerEnded();
                }
                return Convert.ToBoolean(_countSeconds);
            });
        }

        void TimerEnded()
        {
            var activeTask = Data.Databank.Tasks.Where(x => x.active == true).ToList()[0];

            if(current == "pomodoro")
            {
                activeTask.CurrPom += 1;
                Data.Databank.updateTask(activeTask);
            }

            UpdateListView(ListViewTasks);

            if (isBreak && breakTimes != 2)
            {
                btnPomodoro.BackgroundColor = Color.Transparent;
                btnShortBreak.BackgroundColor = Color.FromRgba(55, 55, 55, 55);
                btnLongBreak.BackgroundColor = Color.Transparent;
                lblTimer.Text = "05 : 00";
            }
            else if (isBreak && breakTimes == 2)
            {
                btnPomodoro.BackgroundColor = Color.Transparent;
                btnShortBreak.BackgroundColor = Color.Transparent;
                btnLongBreak.BackgroundColor = Color.FromRgba(55, 55, 55, 55);
                lblTimer.Text = "10 : 00";
            }
            else if (isBreak == false)
            {
                btnPomodoro.BackgroundColor = Color.FromRgba(55, 55, 55, 55);
                btnShortBreak.BackgroundColor = Color.Transparent;
                btnLongBreak.BackgroundColor = Color.Transparent;
                lblTimer.Text = "25 : 00";
            }

            if (isBreak && breakTimes != 2)
            {

                breakTimes++;
                current = "shortBreak";
                isBreak = false;
            }
            else if (isBreak && breakTimes == 2)
            {

                breakTimes = 0;
                current = "longBreak";
                isBreak = false;
            }
            else if (isBreak == false)
            {

                current = "pomodoro";
                isBreak = true;

            }

            switch (current)
            {
                case "pomodoro":
                    _countSeconds = 10;
                    break;
                case "shortBreak":
                    _countSeconds = 5;
                    break;
                case "longBreak":
                    _countSeconds = 7;
                    break;
            }

            timerRunning = true;
            btnStartTimer.Text = "Stop";
           

        }

        private void btnPomodoro_Clicked(object sender, EventArgs e)
        {
            btnPomodoro.BackgroundColor = Color.FromRgba(55, 55, 55, 55);
            btnShortBreak.BackgroundColor = Color.Transparent;
            btnLongBreak.BackgroundColor = Color.Transparent;

            isBreak = false;
            lblTimer.Text = "25 : 00";
            btnStartTimer.Text = "Start";
            if (timerRunning)
            {
                cancelTimer = true;
            }
            
        }

        private void btnShortBreak_Clicked(object sender, EventArgs e)
        {
            btnPomodoro.BackgroundColor = Color.Transparent;
            btnShortBreak.BackgroundColor = Color.FromRgba(55, 55, 55, 55);
            btnLongBreak.BackgroundColor = Color.Transparent;

            isBreak = true;
            if(breakTimes + 1 < 2)
            {
                breakTimes++;
            }
            else
            {
                breakTimes = 0;
            }
            lblTimer.Text = "05 : 00";
            btnStartTimer.Text = "Start";
            if (timerRunning)
            {
                cancelTimer = true;
            }
        }

        private void btnLongBreak_Clicked(object sender, EventArgs e)
        {

            btnPomodoro.BackgroundColor = Color.Transparent;
            btnShortBreak.BackgroundColor = Color.Transparent;
            btnLongBreak.BackgroundColor = Color.FromRgba(55, 55, 55, 55);

            isBreak = true;
            breakTimes = 2;
            lblTimer.Text = "10 : 00";
            btnStartTimer.Text = "Start";
            if (timerRunning)
            {
                cancelTimer = true;
            }
        }

        private void btnDeleteTask_Clicked(object sender, EventArgs e)
        {

            var pomTask = (Data.PomTask)((Button)sender).BindingContext;
            Data.Databank.deleteTask(pomTask);
            Data.Databank.Tasks.Remove(pomTask);
            UpdateListView(ListViewTasks);

        }
    }
}
