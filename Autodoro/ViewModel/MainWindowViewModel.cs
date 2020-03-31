using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodoro.Model;

namespace Autodoro.ViewModel
{
    public class MainWindowViewModel
    {
        public MainWindowModel Model { get; set; }

        public MainWindowViewModel()
        {
            Model = new MainWindowModel()
            {
                LastBreakTime = DateTime.Now,
                LastActivityTime = DateTime.Now,
                IsBreakTime = false,
                FlashMessage = "Focus"
            };
        }

    }
}
