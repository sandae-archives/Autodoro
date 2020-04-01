using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Deployment.Application;

namespace Autodoro.Lib
{
    public class Util
    {
        public static string GetAppVersion()
        {
            try
            {
                return ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString();
            } catch (InvalidDeploymentException)
            {
                return "0.0.0";
            }
        }
    }
}
