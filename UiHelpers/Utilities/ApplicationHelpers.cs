using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using Common.Ui;
using TestStack.White.UIItems.WindowItems;
using TestStack.White;

namespace Common.Utilities
{
  public class ApplicationHelpers
  {
    private Logger logger;

    public ApplicationHelpers()
    {
      logger = new Logger();
    }

    public Window GetMainWindow(string processName, string windowName)
    {
      try
      {
        logger.LogInfo("Finding window {0} in {1}", windowName, processName);
        Application novaApp = GetApplication(processName);
        WaitHelpers.WaitFor(() => WindowsForProcess(processName).Contains(windowName), TimeSpan.FromMinutes(30), TimeSpan.FromMinutes(1));
        WaitHelpers.WaitSeconds(10);
        // WaitHelpers.WaitFor(() => novaApp.GetWindows().Count > 0, TimeSpan.FromMinutes(30));
        logger.LogInfo("Window for {0} is found.", novaApp.Name);
        Window window = novaApp.GetWindow(windowName);
        logger.LogInfo("Nova window {0} is found", window.Title);
        return window;
      }
      catch (Exception e)
      {
        logger.LogError("GetMainWindow failed with exception: {0}. \nInner exception {1}", e.Message,
          e.InnerException.Message);
        throw;
      }
    }

    public List<string> WindowsForProcess(string processName)
    {
      try
      {
        Application application = GetApplication(processName);
        logger.LogInfo("Application found for {0}.", processName);
        List<Window> windows = application.GetWindows();
        logger.LogInfo("{0} windows is found for {1}", windows.Count, processName);

        return windows.Select(w => w.Title).ToList();
      }
      catch (Exception e)
      {
        logger.LogError("Windows for process failed with exception {0}", e.Message);
        throw;
      }
    }

    public bool ProcessIsRunning(string processName)
    {
      Process[] processes = Process.GetProcessesByName(processName);
      if (processes.Any())
      {
        logger.LogInfo("Process {0} is running.", processName);
        return true;
      }

      return false;
    }

    public bool WindowIsVisible(string processName, string windowTitle)
    {
      Application application = GetApplication(processName);

     while (application.GetWindows() != null && !application.GetWindows().Any(e => e.Title == windowTitle))
      {
        logger.LogInfo("{0} windows found for {1}", application.GetWindows().Count, application.Name);
        Thread.Sleep(TimeSpan.FromSeconds(5));
        foreach (var window in WindowsForProcess(MeatMaster2UiItems.Mm2ProcessName))
        {
          logger.LogInfo("Window with name: {0} found", window);
        }
      }

      return true;
    }

    public void KillProcess(string processName)
    {
      if (ProcessIsRunning(processName))
      {
        Process.GetProcessesByName(processName).First().Kill();
      }
    }

    public Application GetApplication(string processName)
    {
      Process process = Process.GetProcessesByName(processName).First();
      Application app = Application.Attach(process);
      logger.LogInfo("Process {0} found.", app.Name);
      return app;
    }

    public void StartApplication(string path)
    {
      Application.Launch(path);
    }

    public void RestartWindows()
    {
      Process.Start("shutdown.exe", "-r -t 0");
    }

    public void StartCaffeine()
    {
      if (!ProcessIsRunning("caffeine"))
      {
        StartApplication("caffeine.exe");
      }
    }
  }
}
