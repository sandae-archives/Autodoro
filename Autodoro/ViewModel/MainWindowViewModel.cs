using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using Autodoro.Model;
using Autodoro.Lib;
using Gma.System.MouseKeyHook;
using LiteDB;

namespace Autodoro.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private string activityName = "Focus";
        private string duration = "00:00";
        private int pomodoroCount = 0;

        private Pomodoro Pomodoro { get; set; }

        public string AppVersion { get; private set; } = "Autodoro - v" + Util.GetAppVersion();

        public MainWindowViewModel()
        {
            Pomodoro = new Pomodoro();

            var repo = new LogRepository();

            PomodoroCount = repo.FindAllToday().Count();


            Pomodoro.BreakTimeRaised += (s, e) =>
            {
                ActivityName = "Break";

                repo.Add(new Log
                {
                    Activity = "Focus",
                    StartTime = DateTime.Now.Subtract(new TimeSpan(0, 25, 0)),
                    EndTime = DateTime.Now
                });

                PomodoroCount = repo.FindAllToday().Count();

                OnBreakTimeRaised(new EventArgs());
            };

            Pomodoro.WorkTimeRaised += (s, e) =>
            {
                ActivityName = "Focus";
                OnWorkTimeRaised(new EventArgs());
            };

            var timer = new DispatcherTimer()
            {
                Interval = new TimeSpan(0, 0, 1)
            };

            timer.Tick += (sender, e) =>
            {
                Pomodoro.Update();

                TimeSpan currentDuration = Pomodoro.GetCurrentDuration();
                Duration = string.Format(
                    "{0}:{1}",
                    currentDuration.Minutes.ToString().PadLeft(2, '0'),
                    currentDuration.Seconds.ToString().PadLeft(2, '0')
                );
            };

            timer.Start();

            Hook.GlobalEvents().KeyDown += (sender, e) => Pomodoro.LastActivityTime = DateTime.Now;
            Hook.GlobalEvents().KeyPress += (sender, e) => Pomodoro.LastActivityTime = DateTime.Now;
            Hook.GlobalEvents().MouseMove += (sender, e) => Pomodoro.LastActivityTime = DateTime.Now;
        }

        public int PomodoroCount
        {
            get
            {
                return pomodoroCount;
            }
            set
            {
                pomodoroCount = value;
                OnPropertyChanged(nameof(PomodoroCount));
            }
        }

        public string Duration
        {
            get
            {
                return duration;
            }
            set
            {
                duration = value;
                OnPropertyChanged(nameof(Duration));
            }
        }


        public string ActivityName
        {
            get
            {
                return activityName;
            }
            set
            {
                activityName = value;
                OnPropertyChanged(nameof(ActivityName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event EventHandler BreakTimeRaised;
        protected virtual void OnBreakTimeRaised(EventArgs e)
        {
            BreakTimeRaised?.Invoke(this, e);
        }

        public event EventHandler WorkTimeRaised;
        protected virtual void OnWorkTimeRaised(EventArgs e)
        {
            WorkTimeRaised?.Invoke(this, e);
        }
    }
}
