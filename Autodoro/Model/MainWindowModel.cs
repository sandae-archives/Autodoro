using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Autodoro.Model
{
    public class MainWindowModel : INotifyPropertyChanged
    {
        const int ALLOWED_IDLE_MINUTES = 5;
        const int BREAK_MINUTES = 1;
        const int WORK_MINUTES = 1;

        private bool isBreakTime;
        private DateTime lastBreakTime;
        private DateTime lastActivityTime;
        private string flashMessage;
        private string duration;

        public void UpdateDuration()
        {
            TimeSpan diff = DateTime.Now.Subtract(LastBreakTime);
            Duration = string.Format(
                "{0}:{1}", 
                diff.Minutes.ToString().PadLeft(2, '0'), 
                diff.Seconds.ToString().PadLeft(2, '0')
            );
        }


        public void Update()
        {
            if (!IsBreakTime)
            {
                TimeSpan beenIdle = DateTime.Now.Subtract(LastActivityTime);
                if (beenIdle.Minutes >= ALLOWED_IDLE_MINUTES)
                {
                    LastBreakTime = DateTime.Now;
                }

                TimeSpan beenWorking = DateTime.Now.Subtract(LastBreakTime);
                if (beenWorking.Minutes >= WORK_MINUTES)
                {
                    OnBreakTimeRaised(new EventArgs());
                    LastBreakTime = DateTime.Now;
                    IsBreakTime = true;
                }
            }

            if (IsBreakTime)
            {
                TimeSpan diff = DateTime.Now.Subtract(LastBreakTime);
                if (diff.Minutes >= BREAK_MINUTES)
                {
                    OnWorkTimeRaised(new EventArgs());
                    LastBreakTime = DateTime.Now;
                    IsBreakTime = false;
                }
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


        public bool IsBreakTime
        {
            get
            {
                return isBreakTime;
            }
            set
            {
                isBreakTime = value;
                OnPropertyChanged(nameof(IsBreakTime));
            }
        }

        public DateTime LastBreakTime
        {
            get
            {
                return lastBreakTime;
            }
            set
            {
                lastBreakTime = value;
                OnPropertyChanged(nameof(LastBreakTime));
            }
        } 

        public DateTime LastActivityTime
        {
            get
            {
                return lastActivityTime;
            }
            set
            {
                lastActivityTime = value;
                OnPropertyChanged(nameof(LastActivityTime));
            }
        }

        public string FlashMessage
        {
            get
            {
                return flashMessage;
            }
            set
            {
                flashMessage = value;
                OnPropertyChanged(nameof(FlashMessage));
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
