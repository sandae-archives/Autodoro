using System;
using System.ComponentModel;
using System.Windows.Threading;

namespace Autodoro.Model
{
    public class PomodoroTimer : INotifyPropertyChanged
    {
        private readonly DispatcherTimer _dispatcherTimer;
        private readonly TimeSpan _limit;
        private TimeSpan _duration;
        private bool _isDone;

        private bool _isStarted;
        private DateTime _startDate;

        private PomodoroTimer(string name, TimeSpan limit)
        {
            Name = name;
            IsBreak = Name == "Break";
            _limit = limit;
            _dispatcherTimer = new DispatcherTimer {Interval = new TimeSpan(0, 0, 1)};
        }

        public static PomodoroTimer PomodoroBreak => new PomodoroTimer("Break", new TimeSpan(0, 1, 0));
        public static PomodoroTimer PomodoroFocus => new PomodoroTimer("Focus", new TimeSpan(0, 2, 0));

        public string Name { get; }
        public bool IsBreak { get; }

        public TimeSpan Duration
        {
            get => _duration;
            private set
            {
                _duration = value;
                OnPropertyChanged(nameof(Duration));
            }
        }

        public bool IsDone
        {
            get => _isDone;
            private set
            {
                _isDone = value;
                OnPropertyChanged(nameof(IsDone));
            }
        }

        public bool IsStarted
        {
            get => _isStarted;
            private set
            {
                _isStarted = value;
                OnPropertyChanged(nameof(IsStarted));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void Start()
        {
            if (_isStarted)
                throw new Exception("Timer is already running");

            IsDone = false;
            _startDate = DateTime.Now;

            _dispatcherTimer.Tick += (s, e) =>
            {
                var duration = DateTime.Now.Subtract(_startDate);
                Duration = duration;

                if (Duration >= _limit)
                {
                    IsDone = true;
                    OnTimerDoneRaised(new EventArgs());
                    Stop();
                }
            };

            _dispatcherTimer.Start();
            _isStarted = true;
        }

        public void Stop()
        {
            if (!_isStarted)
                throw new Exception("Timer is not running");

            _dispatcherTimer.Stop();
            _isStarted = false;
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event EventHandler TimerDoneRaised;

        protected virtual void OnTimerDoneRaised(EventArgs e)
        {
            TimerDoneRaised?.Invoke(this, e);
        }
    }
}