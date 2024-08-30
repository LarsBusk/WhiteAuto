using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
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
        WaitHelpers.WaitFor(() => WindowsForProcess(processName).Contains(windowName), TimeSpan.FromMinutes(30),
          TimeSpan.FromMinutes(1));
        WaitHelpers.WaitSeconds(10);
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
        return null;
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
      try
      {
        Application application = GetApplication(processName);
        var windows = WindowsForProcess(processName);

        while (windows != null && !windows.Any(w => w.Equals(windowTitle)))
        {
          logger.LogInfo("{0} windows found for {1}", application.GetWindows().Count, application.Name);
          Thread.Sleep(TimeSpan.FromSeconds(5));

          foreach (var window in windows)
          {
            logger.LogInfo($"Window with name: {window} found");
          }

          windows = WindowsForProcess(processName);
        }

        return true;
      }
      catch (Exception e)
      {
        logger.LogError(e.Message);
        return false;
      }
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
      try
      {
        Process[] processes = Process.GetProcessesByName(processName);
        logger.LogInfo($"{processes.Length} found");
        Process process = processes.First();
        logger.LogInfo($"Process with name {process.ProcessName} found.");
        Application app = Application.Attach(process);
        logger.LogInfo("Process {0} found.", app.Name);
        return app;
      }
      catch (Exception e)
      {
        logger.LogError(e.Message);
        throw;
      }
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

    public void StartMM2()
    {
      StartApplication(MeatMaster2UiItems.PathToMeatMaster);
      WaitHelpers.WaitFor(() => ProcessIsRunning(MeatMaster2UiItems.Mm2ProcessName), TimeSpan.FromMinutes(2));
      WaitHelpers.WaitFor(
        () => WindowIsVisible(MeatMaster2UiItems.Mm2ProcessName, MeatMaster2UiItems.Mm2MainWindowName),
        TimeSpan.FromMinutes(2));
    }

    public void CleanFolder(string name, DateTime fromDateTime)
    {
      string[] files = Directory.GetFiles(name);
      foreach (var file in files)
      {
        try
        {
          if (File.GetLastWriteTime(file) < fromDateTime)
            File.Delete(file);
        }
        catch (Exception e)
        {
          logger.LogError("File could not be deleted.");
        }
      }
    }

    public void RestartService(string serviceName)
    {
      ServiceController sc = new ServiceController(serviceName);
      logger.LogInfo($"Stopping {serviceName}");
      sc.Stop();

      if (WaitHelpers.WaitForServiceState(sc, ServiceControllerStatus.Stopped))
        logger.LogInfo($"{serviceName} was successfully stopped.");
      else
        logger.LogError($"Failed stopping {serviceName}.");
      
      logger.LogInfo($"Starting {serviceName}");
      sc.Start();

      if (WaitHelpers.WaitForServiceState(sc, ServiceControllerStatus.Running))
        logger.LogInfo($"{serviceName} was successfully started.");
      else
        logger.LogError($"Failed starting {serviceName}");
    }
  }
}
