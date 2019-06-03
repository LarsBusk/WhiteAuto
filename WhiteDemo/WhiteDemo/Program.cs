using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using Castle.Components.DictionaryAdapter;
using Castle.Core.Internal;
using TestStack.White;
using TestStack.White.InputDevices;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.ListBoxItems;
using TestStack.White.UIItems.MenuItems;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.WindowsAPI;
using System.Runtime.InteropServices;

namespace WhiteDemo
{
  class Program
  {
    private static Window mainWindow;
    private static FiWinRunnerWrapper fiwinrunner;

    private Logger logger;

    static void Main(string[] args)
    {
      fiwinrunner = new FiWinRunnerWrapper();
      //logger = new Logger();
      try
      {
        fiwinrunner.GetStuff();
      }
      catch (Exception e)
      {
        //logger.Log(e.Message);
        throw;
      }

      Console.Read();
    }

    private static void SendKeysToMenu(string[] keys)
    {
      AttachedKeyboard keyboard = mainWindow.Keyboard;
      //Log("Sending ALT");
      keyboard.HoldKey(KeyboardInput.SpecialKeys.ALT);
      foreach (var key in keys)
      {
        //logger.Log($"Sending {key}");
        keyboard.Enter(key);
        Thread.Sleep(200);
      }

      keyboard.LeaveKey(KeyboardInput.SpecialKeys.ALT);
      Thread.Sleep(1000);
      //logger.Log($"{mainWindow.Title}");
    }





   /*private List<IntPtr> GetChildWindows(IntPtr parent)
    {
      List<IntPtr> result = new List<IntPtr>();
      GCHandle listHandle = GCHandle.Alloc(result);
      try
      {
        EnumWindowProc childProc = new EnumWindowProc(parent, null);
        EnumChildWindows(parent, childProc, GCHandle.ToIntPtr(listHandle));
      }
      finally
      {
        if (listHandle.IsAllocated)
          listHandle.Free();
      }
      return result;
    }*/



    private static void KillEmAll()
    {
      Process[] processes =
        Process.GetProcesses()
          .FindAll(
            p => p.ProcessName.StartsWith("FWO") | p.ProcessName.StartsWith("FCS") | p.ProcessName.StartsWith("WSF"));

      foreach (var process in processes)
      {
        //logger.Log($"Killing {process.ProcessName}...");
        process.Kill();
      }
    }





    private static WindowPattern GetWindowPattern(AutomationElement targetControl)
    {
      WindowPattern windowPattern = null;

      try
      {
        windowPattern =
          targetControl.GetCurrentPattern(WindowPattern.Pattern)
            as WindowPattern;
      }
      catch (InvalidOperationException)
      {
        // object doesn't support the WindowPattern control pattern
        return null;
      }
      // Make sure the element is usable.
      if (false == windowPattern.WaitForInputIdle(10000))
      {
        // Object not responding in a timely manner
        return null;
      }
      return windowPattern;
    }

    private static void CloseWindow(WindowPattern windowPattern)
    {
      try
      {
        windowPattern.Close();
      }
      catch (InvalidOperationException)
      {
        // object is not able to perform the requested action
        return;
      }
    }
  }
}
