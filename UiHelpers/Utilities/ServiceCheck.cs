using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Common.Utilities
{
  public class ServiceCheck
  {
    public const string MeatMasterServiceName = "MeatMasterIIService";

    public const string HostingserviceName = "MeatMasterII";

    public const string NetTcpPortSharingName = "NetTcpPortSharing";

    public const string ServiceManagerServiceName = "MeatMasterIIServiceManager";

    private Logger logger;

    public ServiceCheck()
    {
      logger = new Logger();
    }


    public void CheckServices()
    {
      bool mm2State = ServiceIsRunning(MeatMasterServiceName);

      bool hostingState = ServiceIsRunning(HostingserviceName);

      logger.LogInfo("Hosting is running: {0}, Meatmaster2 is running: {1}", hostingState, mm2State);
    }

    public void WaitForServices(int timeOutMinutes)
    {
      logger.LogInfo("Waiting for services to start.");

      WaitForServiceToStart(NetTcpPortSharingName, timeOutMinutes);

      WaitForServiceToStart(HostingserviceName, timeOutMinutes);

      WaitForServiceToStart(MeatMasterServiceName, timeOutMinutes);
    }

    public void WaitForServiceToStart(string serviceName, int timeOutMinutes)
    {
      logger.LogInfo("Waiting for {0} service to start.", serviceName);

      if (!WaitHelpers.WaitFor(() => ServiceIsRunning(serviceName), TimeSpan.FromMinutes(timeOutMinutes), TimeSpan.FromSeconds(30)))
      {
        logger.LogError("{0} Service did not start in time.", serviceName);
      }
      else
      {
        logger.LogInfo("{0} Service started.", serviceName);
      }
    }

    public bool ServiceIsRunning(string serviceName)
    {
      ServiceController controller = new ServiceController(serviceName);

      return controller.Status == ServiceControllerStatus.Running;
    }
  }
}
