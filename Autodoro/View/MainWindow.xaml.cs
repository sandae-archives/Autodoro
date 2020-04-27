using System;
using System.Media;
using System.Windows;
using Windows.UI.Notifications;
using Autodoro.Model;
using Autodoro.ViewModel;

namespace Autodoro
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string APP_ID = "Sandaemc.Autodoro";
        private readonly SoundPlayer ding = new SoundPlayer(Properties.Resources.ding);
        private readonly ToastNotifier toaster = ToastNotificationManager.CreateToastNotifier(APP_ID);

        public MainWindow()
        {
            var pomodoroBreak = PomodoroTimer.PomodoroBreak();

            pomodoroBreak.TimerDoneRaised += (s, e) => { Console.WriteLine(pomodoroBreak.Duration.Minutes); };

            pomodoroBreak.Start();


            InitializeComponent();

            var vm = new MainWindowViewModel();

            DataContext = vm;


            // Show the toast. Be sure to specify the AppUserModelId on your application's shortcut!

            vm.BreakTimeRaised += (s, e) =>
            {
                ding.Play();

                var toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastImageAndText03);

                var toast = new ToastNotification(toastXml);
                toast.Tag = Guid.NewGuid().ToString();
                toast.Group = "AutodoroNotification";
                toast.ExpirationTime = DateTime.Now.AddSeconds(30);


                var stringElements = toastXml.GetElementsByTagName("text");
                stringElements[0].AppendChild(toastXml.CreateTextNode("Break Time"));
                stringElements[1].AppendChild(toastXml.CreateTextNode("Standup and take a walk"));

                toaster.Show(toast);

                Show();
                Activate();
                Focus();
            };
        }
    }
}