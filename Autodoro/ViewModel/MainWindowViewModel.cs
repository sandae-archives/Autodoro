using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Threading;
using Autodoro.Lib;
using Autodoro.Model;
using Gma.System.MouseKeyHook;

namespace Autodoro.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private string activityName = "Focus";
        private string duration = "00:00";
        private int pomodoroCount;
        private PomodoroTimer pomodoroTimer;

        public MainWindowViewModel()
        {
            PomodoroTimer = GetNextPomodoro(null);
            PomodoroTimer.TimerDoneRaised += (s, e) => { Console.WriteLine(PomodoroTimer.Name); };

            UpdateCommand = new Updater();

            PomodoroTimer.Start();

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

            var timer = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 1)
            };

            timer.Tick += (sender, e) =>
            {
                Pomodoro.Update();

                var currentDuration = Pomodoro.GetCurrentDuration();
                Duration = string.Format(
                    "{0}:{1}",
                    currentDuration.Minutes.ToString().PadLeft(2, '0'),
                    currentDuration.Seconds.ToString().PadLeft(2, '0')
                );
            };

            timer.Start();

            Hook.GlobalEvents().KeyDown += (sender, e) => Pomodoro.HadMovement();
            Hook.GlobalEvents().KeyPress += (sender, e) => Pomodoro.HadMovement();
            Hook.GlobalEvents().MouseMove += (sender, e) => Pomodoro.HadMovement();
        }

        public ICommand UpdateCommand { get; }

        private Pomodoro Pomodoro { get; }

        public string AppVersion { get; } = "Autodoro - v" + Util.GetAppVersion();

        public PomodoroTimer PomodoroTimer
        {
            get => pomodoroTimer;
            set
            {
                pomodoroTimer = value;
                OnPropertyChanged(nameof(PomodoroTimer));
            }
        }

        public int PomodoroCount
        {
            get => pomodoroCount;
            set
            {
                pomodoroCount = value;
                OnPropertyChanged(nameof(PomodoroCount));
            }
        }

        public string Duration
        {
            get => duration;
            set
            {
                duration = value;
                OnPropertyChanged(nameof(Duration));
            }
        }


        public string ActivityName
        {
            get => activityName;
            set
            {
                activityName = value;
                OnPropertyChanged(nameof(ActivityName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

#nullable enable
        public PomodoroTimer GetNextPomodoro(PomodoroTimer? current)
        {
            return current == null || current.IsBreak
                ? PomodoroTimer.PomodoroFocus()
                : PomodoroTimer.PomodoroBreak();
        }
#nullable disable

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

        internal class Updater : ICommand
        {
            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                Console.WriteLine(parameter);
                Console.WriteLine("Executing");
            }
        }
    }
}