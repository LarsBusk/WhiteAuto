using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using Common.Ui;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;

namespace Common.Utilities
{
  public class MeatMaster2Functions : Functions
  {
    private ApplicationHelpers applicationHelpers;

    public MeatMaster2Functions()
      : base(Uis.MeatMaster2)
    {
      applicationHelpers = new ApplicationHelpers();
    }

    /// <summary>
    /// Waits for the reference splash to disapear in MM2 UI
    /// </summary>
    public void WaitForReference()
    {
      try
      {
        Label referenceLabel = MainWindow.Get<Label>(SearchCriteria.ByText(Properties.Resources.MeatMasterII_ReferenceLabelName));
        WaitHelpers.WaitForVisible(referenceLabel, TimeSpan.FromSeconds(60));
        WaitHelpers.WaitFor(() => !referenceLabel.Visible, TimeSpan.FromMinutes(10), TimeSpan.FromSeconds(30));
      }
      catch (Exception e)
      {
        logger.LogError($"WaitForReference failed with exception {e.Message} inner: {e.InnerException}");
        throw;
      }

    }

    /// <summary>
    /// Cancels startup and leaves X-ray off.
    /// </summary>
    public void CancelStartup()
    {
      WaitHelpers.WaitForDialog(MainWindow, Properties.Resources.MeatMasterII_InstrumentStartupDialog, TimeSpan.FromMinutes(5));

      MainWindow.ModalWindow(Properties.Resources.MeatMasterII_InstrumentStartupDialog)
        .Get<Button>(SearchCriteria.ByText(MeatMaster2UiItems.InstrumentStartupCancelButton))
        .Click();
    }
    /// <summary>
    /// Closes down Nova
    /// </summary>
    /// <param name="restartWindows">If true, windows will restart during the shutdown, before it is done.</param>
    public void CloseDown(bool restartWindows)
    {
      MainWindow.Get<RadioButton>(SearchCriteria.ByText(MeatMaster2UiItems.CareViewButon)).Click();
      Button closeButton = MainWindow.Get<Button>(SearchCriteria.ByText(MeatMaster2UiItems.CloseDownButton));
      WaitHelpers.WaitForEnabled(closeButton, TimeSpan.FromSeconds(30));

      closeButton.Click();

      WaitHelpers.WaitForDialog(MainWindow, MeatMaster2UiItems.MessageBoxPopup, TimeSpan.FromMinutes(2));

      MainWindow.ModalWindow(MeatMaster2UiItems.MessageBoxPopup)
        .Get<Button>(SearchCriteria.ByText(MeatMaster2UiItems.PopupLeftButton))
        .Click();

      if (restartWindows)
      {
        applicationHelpers.RestartWindows();
      }

      WaitHelpers.WaitFor(() => !applicationHelpers.ProcessIsRunning(MeatMaster2UiItems.Mm2ProcessName),
        TimeSpan.FromMinutes(20), TimeSpan.FromMinutes(1));
    }

    /// <summary>
    /// Starts the instrument diagnostic, and cancels after waitTime
    /// </summary>
    /// <param name="waitTime">Time to let the test run before canceling</param>
    public void CancelStartup(TimeSpan waitTime)
    {
      WaitHelpers.WaitForDialog(MainWindow, Properties.Resources.MeatMasterII_InstrumentStartupDialog, TimeSpan.FromMinutes(5));

      Window instrumentDiagnosticDialog = MainWindow.ModalWindow(Properties.Resources.MeatMasterII_InstrumentStartupDialog);

      instrumentDiagnosticDialog.Get<Button>(SearchCriteria.ByText(MeatMaster2UiItems.InstrumentStartupStartButton))
        .Click();

      WaitHelpers.WaitForDialog(MainWindow, MeatMaster2UiItems.InstrumentDiagnosticDialog, TimeSpan.FromSeconds(30));

      Button instrumentDiagnosticCancelButton = MainWindow.ModalWindow(MeatMaster2UiItems.InstrumentDiagnosticDialog)
        .Get<Button>(SearchCriteria.ByText(MeatMaster2UiItems.InstrumentDiagnosticCancelButton));

      if (WaitHelpers.WaitForEnabled(instrumentDiagnosticCancelButton, TimeSpan.FromSeconds(60)))
      {
        Thread.Sleep(waitTime);
        instrumentDiagnosticCancelButton.Click();
        HandlePopup("yes");
      }
      WaitHelpers.WaitForDialog(MainWindow, MeatMaster2UiItems.InstrumentDiagnosticDialog, TimeSpan.FromSeconds(20));

      Button instrumentDiagnosticOkButton = MainWindow.ModalWindow(MeatMaster2UiItems.InstrumentDiagnosticDialog)
        .Get<Button>(SearchCriteria.ByText(MeatMaster2UiItems.InstrumentDiagnosticOkButton));

      if (WaitHelpers.WaitForVisible(instrumentDiagnosticOkButton, TimeSpan.FromSeconds(60)))
      {
        logger.LogInfo("Button was enabled");
        instrumentDiagnosticOkButton.Click();
      }

      else logger.LogInfo("Button was not enabled");
    }

    /// <summary>
    /// Starts instrument diagnostic waits for finish dialog, and clicks OK.
    /// </summary>
    public void HandleInstrumentDiagnostic()
    {
      WaitHelpers.WaitForDialog(MainWindow, Properties.Resources.MeatMasterII_InstrumentStartupDialog, TimeSpan.FromMinutes(5));

      MainWindow.ModalWindow(Properties.Resources.MeatMasterII_InstrumentStartupDialog)
        .Get<Button>(SearchCriteria.ByText(MeatMaster2UiItems.InstrumentStartupStartButton)).Click();

      WaitHelpers.WaitForDialog(MainWindow, MeatMaster2UiItems.InstrumentDiagnosticDialog, TimeSpan.FromSeconds(30));

      Button instrumentDiagnosticOkButton = MainWindow.ModalWindow(MeatMaster2UiItems.InstrumentDiagnosticDialog)
        .Get<Button>(SearchCriteria.ByText(MeatMaster2UiItems.InstrumentDiagnosticOkButton));

      if (WaitHelpers.WaitForVisible(instrumentDiagnosticOkButton, TimeSpan.FromMinutes(60)))
      {
        logger.LogInfo("Button was enabled");
        instrumentDiagnosticOkButton.Click();
      }

      else logger.LogInfo("Button was not enabled");
    }

    public void ClickApplicationswitchButton()
    {
      Button appSwitchButton =
        MainWindow.Get<Button>(SearchCriteria.ByText(MeatMaster2UiItems.MenuBarApplicationSwitchButton));

      if (WaitHelpers.WaitForEnabled(appSwitchButton, TimeSpan.FromSeconds(60)))
      {
        logger.LogInfo("Application switch button is enabled.");
        appSwitchButton.Click();
        WaitForManta();
      }
      else
        logger.LogError("Application switch was not enabled.");
    }

    public void StartAndWaitForCheckSample()
    {
      Window startCheckSampleDialog = MainWindow.ModalWindow(MeatMaster2UiItems.MessageBoxPopup);

      Button startCsButton =
        startCheckSampleDialog.Get<Button>(SearchCriteria.ByText(MeatMaster2UiItems.PopupRightButton));
      startCsButton.Click();

      WaitHelpers.WaitFor(CheckSampleIsDone, TimeSpan.FromMinutes(5));

      Window removeCsWindow = MainWindow.ModalWindow(MeatMaster2UiItems.MessageBoxPopup);

      removeCsWindow.Get<Button>(SearchCriteria.ByText(MeatMaster2UiItems.PopupLeftButton)).Click();
    }

    private bool CheckSampleIsDone()
    {
      return MainWindow.ModalWindows().Any(w => w.Title == MeatMaster2UiItems.MessageBoxPopup);
    }

    private bool WaitForManta()
    {
      if (!WaitHelpers.WaitFor(
        () => applicationHelpers.ProcessIsRunning(UiItems.MantaProcessname),
        TimeSpan.FromSeconds(60)))
      {
        logger.LogError("Manta process did not start in time");
        return false;
      }

      logger.LogInfo("Manta process started.");

      if (
        !WaitHelpers.WaitFor(
          () => applicationHelpers.WindowIsVisible(UiItems.MantaProcessname, UiItems.MantaWindowtitle),
          TimeSpan.FromSeconds(60)))
      {
        logger.LogError("Manta window did not show within 60 secs.");
        return false;
      }

      logger.LogInfo("Manta started in time.");
      return true;
    }
  }
}
