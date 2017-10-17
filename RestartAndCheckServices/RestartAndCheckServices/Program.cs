using System;
using Common.Ui;
using Common.Utilities;

namespace RestartAndCheckServices
{
  internal class Program
  {
    private static void Main(string[] args)
    {
      ServiceCheck serviceCheck = new ServiceCheck();
      ApplicationHelpers applicationHelpers = new ApplicationHelpers();
      MeatMaster2Functions functions = new MeatMaster2Functions();
      Functions.logger.LogInfo("Program is starting.");

      serviceCheck.WaitForServiceToStart(ServiceCheck.ServiceManagerServiceName, 10);

      WaitHelpers.WaitFor(() => applicationHelpers.ProcessIsRunning(MeatMaster2UiItems.Mm2ProcessName),
        TimeSpan.FromMinutes(15));

      DatabaseHelpers.DeleteLastDataMaintenanceDate();

      functions.CancelStartup();

      functions.CloseDown(false);

      applicationHelpers.RestartWindows();
    }
  }
}
