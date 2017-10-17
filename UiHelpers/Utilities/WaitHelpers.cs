using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TestStack.White.UIItems;
using TestStack.White.UIItems.WindowItems;

namespace Common.Utilities
{
  public  class WaitHelpers
  {
    private static Logger logger = new Logger();

    public WaitHelpers()
    {
      logger = new Logger();
    }

    public static bool WaitFor(Func<bool> condition, TimeSpan timeout, TimeSpan interval)
    {
      DateTime startTime = DateTime.Now;
      bool elapsed = false;

      while (!condition() && !elapsed)
      {
        Thread.Sleep(interval);
        elapsed = (DateTime.Now - startTime) > timeout;
      }

      return !elapsed;
    }

    public static bool WaitFor(Func<bool> condition, TimeSpan timeout)
    {
      return WaitFor(condition, timeout, TimeSpan.FromSeconds(10));
    }

    public static void WaitSeconds(int seconds)
    {
      logger.LogInfo("Waiting for {0} secs.", seconds);
      Thread.Sleep(TimeSpan.FromSeconds(seconds));
    }

    public static bool WaitForVisible(UIItem uiItem, TimeSpan timeout)
    {
      if (uiItem != null)
      {
        logger.LogInfo("Waiting {0} secs for {1} to become visible", timeout.TotalSeconds, uiItem.Name);

        WaitFor(() => uiItem.Visible, timeout);
        
        if (uiItem.Visible)
          logger.LogInfo("{0} became visible in time", uiItem.Name);
        else
          logger.LogError("{0} was not visible after {1} secs.", uiItem.Name, timeout.TotalSeconds);

        return uiItem.Visible;
      }

      logger.LogError("WaitForVisible: UiItem was null");
      return false;
    }

    public static bool WaitForEnabled(UIItem uiItem, TimeSpan timeout)
    {
      return WaitForEnabled(uiItem, timeout, TimeSpan.FromSeconds(10));
    }

    public static bool WaitForEnabled(UIItem uiItem, TimeSpan timeout, TimeSpan interval)
    {
      if (uiItem != null)
      {
        WaitFor(() => uiItem.Enabled, timeout, interval);
        if (uiItem.Enabled)
        {
          logger.LogInfo("{0} was enabled", uiItem.Name);
          return true;
        }

        logger.LogError("{0} did not become enabled within {1} seconds.", uiItem.Name, timeout.TotalSeconds);
        return false;
      }

      logger.LogError("The uiitem was null.");
      return false;
    }

    /// <summary>
    /// Checks if a dialog will show before the given time
    /// </summary>
    /// <param name="window">Parent window of the dialog</param>
    /// <param name="dialogName">Name of the dialog</param>
    /// <param name="timeout">The timespan to wait for the dialog</param>
    /// <returns>true if the dialog shows within the given timespan else return false</returns>
    public static bool WaitForDialog(Window window, string dialogName, TimeSpan timeout)
    {
      if (WaitFor(() => DialogIsFound(window, dialogName), timeout))
      {
        logger.LogInfo("Dialog {0} is found", dialogName);
        return true;
      }

      logger.LogError("Dialog {0} was not found", dialogName);
      return false;
    }

    private static bool DialogIsFound(Window window, string dialogName)
    {
      IEnumerable<Window> dialogs = window.ModalWindows();
      return dialogs.Any(d => d.Name == dialogName);
    }
  }
}
