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
using System.Media;
using System.Windows.Threading;
using Autodoro.ViewModel;

namespace Autodoro
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindowViewModel VM { get; set; } = new MainWindowViewModel();


        public MainWindow()
        {
            InitializeComponent();
            InitTimer();

            VM.Model.BreakTimeRaised += (sender, e) =>
            {
                Show();
                Activate();
                Focus();
                VM.Model.FlashMessage = "Break";
            };

            VM.Model.WorkTimeRaised += (sender, e) =>
            {
                Show();
                Activate();
                Focus();
                VM.Model.FlashMessage = "Focus";
            };

            Hook.GlobalEvents().KeyDown += (sender, e) => VM.Model.LastActivityTime = DateTime.Now;
            Hook.GlobalEvents().KeyPress += (sender, e) => VM.Model.LastActivityTime = DateTime.Now;
            Hook.GlobalEvents().MouseMove += (sender, e) => VM.Model.LastActivityTime = DateTime.Now;

            DataContext = VM;
        }

        private void InitTimer()
        {
            var s = new DispatcherTimer()
            {
                Interval = new TimeSpan(0, 0, 1)
            };

            s.Tick += (sender, e) =>
            {
                VM.Model.Update();
                VM.Model.UpdateDuration();
            };
            s.Start();
        }

    }
}
