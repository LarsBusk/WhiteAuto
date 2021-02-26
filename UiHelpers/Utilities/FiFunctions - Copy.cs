using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Common.Ui;
using Common.Utilities;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;

namespace Common.Utilities
{
  public class FiFunctions
  {
    public Window MainWindow { get; set; }

    private ApplicationHelpers applicationHelpers;

    private Logger logger;

    public FiFunctions()
    {
      this.applicationHelpers = new ApplicationHelpers();
      this.MainWindow = applicationHelpers.GetMainWindow(FossIntegratorUiItems.FiProcessName, FossIntegratorUiItems.FiResultDisplayName);
      logger = new Logger();
    }

    public void ChangeMode(FiMode mode, bool quickChange = false)
    {
      switch (mode)
      {
        case FiMode.Measuring:
          ClickStartButton();
          HandleModeShiftDialog(quickChange);
          WaitForMode(mode, TimeSpan.FromMinutes(5));
          break;
        case FiMode.Standby:
          ClickPauseButton();
          HandleModeShiftDialog(quickChange);
          WaitForMode(mode, TimeSpan.FromMinutes(30));
          break;
        case FiMode.Stop:
          ClickStopButton();
          HandleModeShiftDialog(quickChange);
          WaitForMode(mode, TimeSpan.FromMinutes(45));
          break;
      }
    }


    public string GetMode()
    {
      Label modeTextBox = MainWindow.Get<Label>(SearchCriteria.ByAutomationId(FossIntegratorUiItems.ModeTextAutoId));
      return modeTextBox.Name;
    }

    public bool StartButtonIsEnabled()
    {
      Button startButton = MainWindow.Get<Button>(SearchCriteria.ByText(FossIntegratorUiItems.StartButtonName));
      return startButton.Enabled;
    }

    private void ClickStartButton()
    {
      Button button = MainWindow.Get<Button>(SearchCriteria.ByText(FossIntegratorUiItems.StartButtonName));

      while (!button.Enabled)
      {
        Thread.Sleep(TimeSpan.FromSeconds(30));
      }

      button.Click();
    }

    private void ClickPauseButton()
    {
      Button button = MainWindow.Get<Button>(SearchCriteria.ByText(FossIntegratorUiItems.PauseButtonName));

      while (!button.Enabled)
      {
        Thread.Sleep(TimeSpan.FromSeconds(30));
      }

      button.Click();
    }

    private void ClickStopButton()
    {
      Button button = MainWindow.Get<Button>(SearchCriteria.ByText(FossIntegratorUiItems.StopButtonName));

      while (!button.Enabled)
      {
        Thread.Sleep(TimeSpan.FromSeconds(30));
      }

      button.Click();
    }

    private bool StandByButtonIsEnabled()
    {
      Button startButton = MainWindow.Get<Button>(SearchCriteria.ByText(FossIntegratorUiItems.StartButtonName));
      return startButton.Enabled;
    }

    private bool StopButtonIsEnabled()
    {
      Button startButton = MainWindow.Get<Button>(SearchCriteria.ByText(FossIntegratorUiItems.StopButtonName));
      return startButton.Enabled;
    }

    private void HandleModeShiftDialog(bool quickModeChange)
    {
      if (WaitHelpers.WaitFor(
          () => MainWindow.ModalWindows().Any(d => d.Name.StartsWith(FossIntegratorUiItems.ModeChangeDialogTitle)),
          TimeSpan.FromMinutes(1)))
      {
        logger.LogInfo("Modeshift dialog found, clicking Ok.");
        Window modeShiftDialog =
          MainWindow.ModalWindows().Find(d => d.Name.StartsWith(FossIntegratorUiItems.ModeChangeDialogTitle));
        if (quickModeChange)
        {
          CheckBox quickChangeCheckBox =
            modeShiftDialog.Get<CheckBox>(
              SearchCriteria.ByAutomationId(FossIntegratorUiItems.ModeChangeQuickChangeCehckBoxAutoId));
          SetCheckBox(quickChangeCheckBox, true);
        }

        modeShiftDialog.Get<Button>(SearchCriteria.ByAutomationId(FossIntegratorUiItems.ModeChangeOkButtonAutoId))
          .Click();
      }
      else
      {
        logger.LogInfo("Modeshift dialog was not found");
      }
    }

    private void SetCheckBox(CheckBox item, bool valueToSet)
    {
      string itemName = item.Name;
      logger.LogInfo($"Setting checkbox '{itemName}' to '{valueToSet}'.");
      if (item.IsSelected == valueToSet)
      {
        logger.LogInfo("CheckBox '{itemName}' already has value '{valueToSet}'.");
        return;
      }

      item.Click();

      // Check result
      if (item.Checked != valueToSet)
      {
        string errorMessage = $"Failed to set CheckBox '{itemName}' to '{valueToSet}'.";
        logger.LogError(errorMessage);
        throw new UIActionException(errorMessage);
      }
    }

    private bool WaitForMode(FiMode mode, TimeSpan timeout)
    {
      logger.LogInfo($"Waiting for mode {mode}");

      if (WaitHelpers.WaitFor(() => GetMode().Equals(mode.ToString()), timeout))
      {
        logger.LogInfo($"Changed to {mode} in time");
        return true;
      }

      return false;
    }

    public void ResetErrors()
    {
      Button resetErrorsButton = MainWindow.Get<Button>(SearchCriteria.ByText(FossIntegratorUiItems.ResetButtonName));

      if (resetErrorsButton.Enabled)
      {
        resetErrorsButton.Click();
      }
    }

    public enum FiMode { Stop, Standby, Measuring }
  }
}
