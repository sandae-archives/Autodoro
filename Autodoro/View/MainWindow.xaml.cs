using System;
using System.Media;
using System.Windows;
using Autodoro.Model;
using Autodoro.ViewModel;
using LiteDB;

namespace Autodoro
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly SoundPlayer ding = new SoundPlayer(Properties.Resources.ding);

        public MainWindow()
        {
            InitializeComponent();

            var vm = new MainWindowViewModel();

            DataContext = vm;

            vm.BreakTimeRaised += (s, e) =>
            {
                ding.Play();
                Show();
                Activate();
                Focus();
            };
        }
    }
}
