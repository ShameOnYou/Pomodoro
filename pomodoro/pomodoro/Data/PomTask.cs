using System;
using System.Collections.Generic;
using System.Text;

namespace pomodoro.Data
{
    class PomTask
    {
        public PomTask()
        {
            CurrPom = 0;
            MaxPom = 1;
            TaskName = "New Task";
            active = false;
            taskId = -1;
        }

        public PomTask(string taskName, int currPom, int maxPom)
        {
            CurrPom = currPom;
            MaxPom = maxPom;
            TaskName = taskName;
        }

        public string TaskName { get; set; }
        public int CurrPom { get; set; }
        public int MaxPom { get; set; }
        public bool active { get; set; }
        public int taskId { get; set; }
        public bool ValidTask => (taskId > -1);

    }
}
