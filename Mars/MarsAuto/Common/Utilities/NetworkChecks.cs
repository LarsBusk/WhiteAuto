using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Common.Utilities
{
  public class NetworkChecks : NetworkInterface
  {
    private Logger logger;

    public NetworkChecks()
    {
      this.logger = new Logger();
    }
    public void WaitForNetwork()
    {
      logger.LogInfo("Checking state of network adapters.");
      foreach (var networkInterface in GetAllNetworkInterfaces())
      {
        do
        {
          logger.LogInfo("{0} is in state {1}", networkInterface.Name, networkInterface.OperationalStatus);
          Thread.Sleep(TimeSpan.FromSeconds(1));
        } while (networkInterface.OperationalStatus != OperationalStatus.Up &&
                  networkInterface.OperationalStatus!=OperationalStatus.Down);
      }
    }

    #region Implementation of abstract class

    public override IPInterfaceProperties GetIPProperties()
    {
      throw new NotImplementedException();
    }

    public override IPv4InterfaceStatistics GetIPv4Statistics()
    {
      throw new NotImplementedException();
    }

    public override PhysicalAddress GetPhysicalAddress()
    {
      throw new NotImplementedException();
    }

    public override string Id
    {
      get { throw new NotImplementedException(); }
    }

    public override bool IsReceiveOnly
    {
      get { throw new NotImplementedException(); }
    }

    public override string Name
    {
      get { throw new NotImplementedException(); }
    }

    public override NetworkInterfaceType NetworkInterfaceType
    {
      get { throw new NotImplementedException(); }
    }

    public override OperationalStatus OperationalStatus
    {
      get { throw new NotImplementedException(); }
    }

    public override long Speed
    {
      get { throw new NotImplementedException(); }
    }

    public override bool Supports(NetworkInterfaceComponent networkInterfaceComponent)
    {
      throw new NotImplementedException();
    }

    public override bool SupportsMulticast
    {
      get { throw new NotImplementedException(); }
    }

    public override string Description
    {
      get { throw new NotImplementedException(); }
    }

    #endregion
  }
}
