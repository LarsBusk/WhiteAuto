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
using TestStack.White.UIItems.ListBoxItems;
using TestStack.White.UIItems.WindowItems;

namespace Common.Utilities
{
  public class MeatMaster2Functions : Functions
  {
    private readonly ApplicationHelpers applicationHelpers;

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
      WaitHelpers.WaitForDialog(MainWindow, Properties.Resources.MeatMasterII_InstrumentStartupDialog,
        TimeSpan.FromMinutes(5));
      var startDialog = MainWindow.ModalWindow(Properties.Resources.MeatMasterII_InstrumentStartupDialog);
      ClickButton(SearchCriteria.ByText(MeatMaster2UiItems.InstrumentStartupCancelButton), startDialog);
    }

    /// <summary>
    /// Closes down Nova
    /// </summary>
    /// <param name="restartWindows">If true, windows will restart during the shutdown, before it is done.</param>
    public void CloseDown(bool restartWindows)
    {
      ChangeToCareView();
      ClickButton(MeatMaster2UiItems.CloseDownButton);

      WaitHelpers.WaitForDialog(MainWindow, MeatMaster2UiItems.MessageBoxPopup, TimeSpan.FromMinutes(2));
      var closeDialog = MainWindow.ModalWindow(MeatMaster2UiItems.MessageBoxPopup);
      ClickButton(MeatMaster2UiItems.PopupLeftButton, closeDialog);

      if (restartWindows)
      {
        applicationHelpers.RestartWindows();
      }

      WaitHelpers.WaitFor(() => !applicationHelpers.ProcessIsRunning(MeatMaster2UiItems.Mm2ProcessName),
        TimeSpan.FromMinutes(20), TimeSpan.FromMinutes(1));
    }

    public void StartDiagnostics()
    {
      ChangeToCareView();
      ClickButton(MeatMaster2UiItems.CareViewInstrumentDiagnosticsButton);
      MainWindow.ModalWindow(MeatMaster2UiItems.MessageBoxPopup)
        .Get<Button>(SearchCriteria.ByText(MeatMaster2UiItems.PopupLeftButton))
        .Click();
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

    public void GetDiagnosticsResult()
    {
      WaitHelpers.WaitForDialog(MainWindow, MeatMaster2UiItems.InstrumentDiagnosticDialog, TimeSpan.FromSeconds(30));
      Button instrumentDiagnosticOkButton = MainWindow.ModalWindow(MeatMaster2UiItems.InstrumentDiagnosticDialog)
        .Get<Button>(SearchCriteria.ByText(MeatMaster2UiItems.InstrumentDiagnosticOkButton));
      Button instrumentDiagnosticRepeatButton = MainWindow.ModalWindow(MeatMaster2UiItems.InstrumentDiagnosticDialog)
        .Get<Button>(SearchCriteria.ByText(MeatMaster2UiItems.InstrumentDiagnosticRepeatButton));

      if (WaitHelpers.WaitForVisible(instrumentDiagnosticOkButton, TimeSpan.FromMinutes(60)))
      {
        if (instrumentDiagnosticRepeatButton.Visible)
        {
          instrumentDiagnosticOkButton.Click();
          CollectLogs();
        }
        else
        {
          logger.LogInfo("diagnostics passed.");
          instrumentDiagnosticOkButton.Click();
        }
      }
      else
      {
        logger.LogInfo("Button was not enabled");
      }
    }

    public void CollectLogs()
    {
      ChangeToCareView();
      ClickButton(MeatMaster2UiItems.CareViewExportLogsButton, MainWindow);

      WaitHelpers.WaitForDialog(MainWindow, MeatMaster2UiItems.ExportLogsDialog, TimeSpan.FromSeconds(30));
      var exportLogsDialog = MainWindow.ModalWindow(MeatMaster2UiItems.ExportLogsDialog);
      
      var combo = exportLogsDialog.Get<ComboBox>(SearchCriteria.All);
      var its = combo.Items;
      foreach (var it in its)
      {
        logger.LogInfo(it.Text);
      }

      int item = combo.Items.FindIndex(i => i.Text.ToUpper().StartsWith("1 D"));
      logger.LogInfo($"The item {item} is found.");
      combo.Select(item);
      
      ClickButton(MeatMaster2UiItems.PopupCollectLogsButton, exportLogsDialog);
      WaitHelpers.WaitForDialog(MainWindow, MeatMaster2UiItems.MessageBoxPopup, TimeSpan.FromMinutes(60));
      var logsExportDoneDialog = MainWindow.ModalWindow(MeatMaster2UiItems.MessageBoxPopup);
      WaitHelpers.WaitFor(() =>
        logsExportDoneDialog.Get<Button>(SearchCriteria.ByText(MeatMaster2UiItems.PopupLeftButton)).Visible, TimeSpan.FromMinutes(30));
      ClickButton(MeatMaster2UiItems.PopupLeftButton, logsExportDoneDialog);
      ClickButton(MeatMaster2UiItems.PopupCloseButton, exportLogsDialog);
    }

    public void SwitchOffXray()
    {
      logger.LogInfo("Switching off X-Ray");
      Window ontopView = ApplicationHelpers.GetApplication(MeatMaster2UiItems.OnTopProcessName)
        .GetWindow(MeatMaster2UiItems.OnTopWindowName);
      
      ClickButton(SearchCriteria.All, ontopView);
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

    public void StartAndWaitForCheckSample(int minutesToWait = 5)
    {
      Window startCheckSampleDialog = MainWindow.ModalWindow(MeatMaster2UiItems.MessageBoxPopup);

      Button startCsButton =
        startCheckSampleDialog.Get<Button>(SearchCriteria.ByText(MeatMaster2UiItems.PopupRightButton));
      startCsButton.Click();

      WaitHelpers.WaitFor(CheckSampleIsDone, TimeSpan.FromMinutes(minutesToWait));

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
