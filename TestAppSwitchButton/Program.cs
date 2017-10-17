
using Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Common.Ui;


namespace TestAppSwitchButton
{
  class Program
  {
    private static ApplicationHelpers applicationHelpers;

    static void Main(string[] args)
    {
      string novaPath = @"C:\Program Files (x86)\FOSS\MeatMaster II\FOSS.Nova.UI.Antonius.Core.exe";
      MeatMaster2Functions helper;

      applicationHelpers = new ApplicationHelpers();

      while (true)
      {
        applicationHelpers.StartApplication(novaPath);
        WaitForNovaWindow();
        helper = new MeatMaster2Functions();

        //helper.HandleInstrumentDiagnostic();

        helper.CancelStartup(TimeSpan.FromSeconds(60));
        helper.SelectProduct("Reference");
        helper.ClickStartStopButton();
        helper.WaitForReference();

        helper.ClickApplicationswitchButton();

        applicationHelpers.KillProcess(UiItems.MantaProcessname);
        applicationHelpers.KillProcess(MeatMaster2UiItems.Mm2ProcessName);

        Thread.Sleep(TimeSpan.FromMinutes(5));
      }
    }

    public static void WaitForNovaWindow()
    {
      while (!applicationHelpers.WindowIsVisible(MeatMaster2UiItems.Mm2ProcessName, MeatMaster2UiItems.Mm2MainWindowName))
      {
        Thread.Sleep(TimeSpan.FromMinutes(2));
      }
    }
  }
}
