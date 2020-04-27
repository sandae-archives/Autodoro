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
            }
            catch (InvalidDeploymentException)
            {
                return "0.0.0.0";
            }
        }
    }
}