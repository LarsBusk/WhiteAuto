using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.Ui;
using Common.Utilities;

namespace MM2CheckStabilityTest
{
  class Program
  {
    static void Main(string[] args)
    {
      ApplicationHelpers helpers = new ApplicationHelpers();

      while (true)
      {
        helpers.CleanFolder(@"C:\Syslog\Wire", DateTime.Now.Subtract(TimeSpan.FromHours(1)));
        helpers.StartMM2();

        var functions = new MeatMaster2Functions();
        functions.CancelStartup();
        functions.StartDiagnostics();
        functions.GetDiagnosticsResult();
        functions.SwitchOffXray();
        functions.CloseDown(restartWindows: false);
        WaitHelpers.WaitSeconds(60);
        //helpers.RestartService("MeatMasterIIServiceManager");
      }
    }
  }
}
