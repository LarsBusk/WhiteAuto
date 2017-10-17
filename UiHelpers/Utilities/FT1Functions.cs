using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Common.Ui;
using TestStack.White;
using TestStack.White.InputDevices;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.MenuItems;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.UIItems.WindowStripControls;
using TestStack.White.WindowsAPI;

namespace Common.Utilities
{
  public class FT1Functions : Functions
  {
    private Window mirrorGainAdjustmentDialog;

    private Window delayAdjustmentDialog;

    private Window noiseTestDialog;

    private Window filterTestDialog;
    
    public FT1Functions() :
      base(Uis.Ft1)
    {
      logger.LogInfo($"Mainwindow {MainWindow.Name} found.");
    }

    public void RunMirrorGainAdjustment()
    {
      StartMirrorGainAdjustment();
      WaitHelpers.WaitSeconds(45);
      ClickRadioButton(FT1UiItems.ShortScanRadioButton, mirrorGainAdjustmentDialog);
      WaitHelpers.WaitSeconds(45);
      ClickButton(FT1UiItems.StoreButton, mirrorGainAdjustmentDialog);
    }

    public void RunDelayAdjustment()
    {
      StartDelayAdjustment();
      ClickButton(FT1UiItems.StartButton, delayAdjustmentDialog);
      ClickButton(FT1UiItems.StoreButton, delayAdjustmentDialog);
    }

    public void RunNoiseTest()
    {
      StartNoiseTest();
      ClickButton(FT1UiItems.StartButton, noiseTestDialog);
      ClickButton(FT1UiItems.ExitButton, noiseTestDialog);
    }

    public void RunFilterTest()
    {
      StartFilterTest();
      WaitHelpers.WaitSeconds(30);
      ClickButton(FT1UiItems.StartButton, filterTestDialog);
      WaitHelpers.WaitSeconds(10);
      ClickButton(FT1UiItems.StoreButton, filterTestDialog);
      WaitHelpers.WaitSeconds(10);
      ClickButton(FT1UiItems.ExitButton, filterTestDialog);
    }
      

    private void StartMirrorGainAdjustment()
    {
      logger.LogInfo("Starting Mirror/Gain adjustment.");
      SendKeysToMenu(new[] { "M", "v", "R", "M" });
      mirrorGainAdjustmentDialog = MainWindow.ModalWindow(FT1UiItems.MirrorGainDialogTitle);
      logger.LogInfo($"Dialog {mirrorGainAdjustmentDialog.Title} found.");
    }

    private void StartDelayAdjustment()
    {
      logger.LogInfo("Starting Delay adjustment.");
      SendKeysToMenu(new[] { "M", "v", "R", "D" });
      delayAdjustmentDialog = MainWindow.ModalWindow(FT1UiItems.DelayAdjustmentTitle);
      logger.LogInfo($"Dialog {delayAdjustmentDialog.Title} found.");
    }

    private void StartNoiseTest()
    {
      logger.LogInfo("Starting noise test.");
      SendKeysToMenu(new[] { "M", "v", "R", "N" });
      noiseTestDialog = MainWindow.ModalWindow(FT1UiItems.NoiseTestTitle);
      logger.LogInfo($"Dialog {noiseTestDialog.Title} found.");
    }

    private void StartFilterTest()
    {
      logger.LogInfo("Starting filter test.");
      SendKeysToMenu(new [] {"M", "v", "R", "F"});
      filterTestDialog = MainWindow.ModalWindow(FT1UiItems.FilterTestTitle);
      logger.LogInfo($"Dialog {filterTestDialog.Title} found.");
    }
    /// <summary>
    /// Navigating the menu by sending ALT and a number of keys
    /// </summary>
    /// <param name="keys">The combination of keys to send
    /// M is maintenance
    /// v is Service
    /// R is service routines these 3 brings you to the service routine menu, the last selects the test
    /// F is filter test
    /// N is noise test
    /// M is mirror/gain adjustment
    /// D is delay adjustment</param>
    private void SendKeysToMenu(string[] keys)
    {
      AttachedKeyboard keyboard = MainWindow.Keyboard;
      keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.ALT);
      foreach (var key in keys)
      {
        keyboard.Enter(key);
      }
    }
  }
}
