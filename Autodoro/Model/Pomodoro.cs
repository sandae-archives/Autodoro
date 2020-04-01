using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Autodoro.Model
{
    public class Pomodoro 
    {
        const int ALLOWED_IDLE_MINUTES = 5;
        const int BREAK_MINUTES = 5;
        const int WORK_MINUTES = 25;

        public bool IsBreakTime { get; private set; }
        public DateTime LastBreakTime { get; private set; } = DateTime.Now;
        public DateTime LastActivityTime { get; set; } = DateTime.Now;

        public TimeSpan GetCurrentDuration()
        {
            return DateTime.Now.Subtract(LastBreakTime);
        }

        public void Update()
        {
            if (IsBreakTime)
            {
                TimeSpan diff = DateTime.Now.Subtract(LastBreakTime);
                if (diff.Minutes >= BREAK_MINUTES)
                {
                    OnWorkTimeRaised(new EventArgs());
                    LastBreakTime = DateTime.Now;
                    IsBreakTime = false;
                }
            } else
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
