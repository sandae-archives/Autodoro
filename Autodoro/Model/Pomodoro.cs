using System;

namespace Autodoro.Model
{
    public class Pomodoro
    {
        private const int ALLOWED_IDLE_MINUTES = 5;
        private const int ALLOWED_IDLE_MINUTES_AFTER_BREAK = 1;

        private const int BREAK_MINUTES = 5;
        private const int WORK_MINUTES = 25;

        private bool didBreak;

        public bool IsBreakTime { get; private set; }
        public DateTime LastBreakTime { get; private set; } = DateTime.Now;
        public DateTime LastActivityTime { get; private set; } = DateTime.Now;

        public void HadMovement()
        {
            if (didBreak)
                didBreak = false;

            LastActivityTime = DateTime.Now;
        }

        public TimeSpan GetCurrentDuration()
        {
            return DateTime.Now.Subtract(LastBreakTime);
        }

        public void Update()
        {
            if (IsBreakTime)
            {
                var diff = DateTime.Now.Subtract(LastBreakTime);
                if (diff.Minutes >= BREAK_MINUTES)
                {
                    OnWorkTimeRaised(new EventArgs());
                    LastBreakTime = DateTime.Now;
                    IsBreakTime = false;
                    didBreak = true;
                }
            }
            else
            {
                if (didBreak)
                {
                    var beenIdleAfterBreak = DateTime.Now.Subtract(LastActivityTime);
                    if (beenIdleAfterBreak.Minutes >= ALLOWED_IDLE_MINUTES_AFTER_BREAK) LastBreakTime = DateTime.Now;
                }

                var beenIdle = DateTime.Now.Subtract(LastActivityTime);
                if (beenIdle.Minutes >= ALLOWED_IDLE_MINUTES) LastBreakTime = DateTime.Now;

                var beenWorking = DateTime.Now.Subtract(LastBreakTime);
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