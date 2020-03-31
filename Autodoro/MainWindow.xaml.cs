using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using Gma.System.MouseKeyHook;
using System.Configuration;
using System.Diagnostics;
using PusherServer;

namespace Autodoro
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const int ALLOWED_IDLE_MINUTES = 5;
        const int BREAK_MINUTES = 5;
        const int WORK_MINUTES = 1;

        private System.Timers.Timer timer;

        private Pusher pusher;

        private bool isBreakTime = false;
        private DateTime lastBreakTime = DateTime.Now;
        private DateTime lastActivityTime = DateTime.Now;

        public MainWindow()
        {
            InitializeComponent();
            InitPusher();
            InitTimer();


            Hook.GlobalEvents().KeyDown += (sender, e) => lastActivityTime = DateTime.Now;
            Hook.GlobalEvents().KeyPress += (sender, e) => lastActivityTime = DateTime.Now;
            Hook.GlobalEvents().MouseMove += (sender, e) => lastActivityTime = DateTime.Now;
        }

        private void InitPusher()
        {
            var options = new PusherOptions
            {
                Cluster = "ap1",
                Encrypted = true
            };

            pusher = new Pusher(
                ConfigurationManager.AppSettings.Get("APP_ID"),
                ConfigurationManager.AppSettings.Get("APP_KEY"),
                ConfigurationManager.AppSettings.Get("APP_SECRET"),
                options
            );
        }

        private void InitTimer()
        {
            timer = new System.Timers.Timer(60000)
            {
                Enabled = true
            };

            timer.Elapsed += (sender, e) =>
            {
                if (!isBreakTime)
                {
                    TimeSpan beenIdle = DateTime.Now.Subtract(lastActivityTime);
                    if (beenIdle.Minutes >= ALLOWED_IDLE_MINUTES)
                    {
                        lastBreakTime = DateTime.Now;
                    }

                    TimeSpan beenWorking = lastActivityTime.Subtract(lastBreakTime);
                    if (beenWorking.Minutes >= WORK_MINUTES)
                    {
                        NotifyBreakTime();
                        lastBreakTime = DateTime.Now.AddMinutes(BREAK_MINUTES);
                        isBreakTime = true;
                    }
                }

                if (isBreakTime)
                {
                    TimeSpan diff = DateTime.Now.Subtract(lastBreakTime);
                    if (diff.Minutes >= BREAK_MINUTES)
                    {
                        isBreakTime = false;
                    }
                }
            };
        }

        private async void NotifyBreakTime()
        {
            await pusher.TriggerAsync(
                "breaktime",
                "break",
                new { message = "Please take a break!" }
            );
        }
    }
}
