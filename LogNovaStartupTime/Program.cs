using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Common.Ui;
using Common.Utilities;

namespace LogNovaStartupTime
{
  internal class Program
  {
    #region Private fields

    private static ServiceCheck serviceCheck;

    private static ApplicationHelpers applicationHelpers;

    private static Logger logger;

    #endregion

    private static void Main(string[] args)
    {
      logger = new Logger();

      logger.LogInfo("Program started");

      NetworkChecks networkChecks = new NetworkChecks();

      serviceCheck = new ServiceCheck();

      applicationHelpers = new ApplicationHelpers();

      networkChecks.WaitForNetwork();

      CheckIfServiceAndProcessIsRunning();

      if (serviceCheck.ServiceIsRunning(ServiceCheck.MeatMasterServiceName))
      {
        logger.LogInfo("Services are running.");
      }

      CancelStartupAndCloseDown();

      Process.Start("shutdown.exe", "-r -t 0");
    }

    #region Private methods

    private static void CheckIfServiceAndProcessIsRunning()
    {
      int i = 0;

      while (i < 100 &
             !(serviceCheck.ServiceIsRunning(ServiceCheck.MeatMasterServiceName) &
               applicationHelpers.ProcessIsRunning(MeatMaster2UiItems.Mm2ProcessName)))
      {
        Thread.Sleep(TimeSpan.FromSeconds(10));

        if (serviceCheck.ServiceIsRunning(ServiceCheck.MeatMasterServiceName))
        {
          logger.LogInfo("Services are running.");
        }

        if (applicationHelpers.ProcessIsRunning(MeatMaster2UiItems.Mm2ProcessName))
        {
          logger.LogInfo("Ui process is running.");
        }

        i++;
        logger.LogInfo("i = {0}", i);
      }

      logger.LogInfo("Returning from service and process check.");
    }

    private static void CancelStartupAndCloseDown()
    {
      if (applicationHelpers.ProcessIsRunning(MeatMaster2UiItems.Mm2ProcessName))
      {
        logger.LogInfo("Ui process is running.");

        while (!applicationHelpers.WindowIsVisible(MeatMaster2UiItems.Mm2ProcessName, MeatMaster2UiItems.Mm2MainWindowName))
        {
          logger.LogInfo("Waiting for window {0}", MeatMaster2UiItems.Mm2MainWindowName);
          Thread.Sleep(TimeSpan.FromSeconds(10));
        }

        logger.LogInfo("MeatMaster mainwindow is visible.");

        MeatMaster2Functions functions = new MeatMaster2Functions();

        functions.CancelStartup(TimeSpan.FromSeconds(10));

        functions.CloseDown();

        logger.LogInfo("Shutting down in 1 min...");

        Thread.Sleep(TimeSpan.FromMinutes(1));
      }
    }

    #endregion
  }
}
