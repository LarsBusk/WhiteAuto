using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Common.Utilities
{
  public class Logger
  {
    private string logFileName;

    public Logger()
    {
      this.logFileName = "AutoTest.log";
      InitiateLog();
    }

    public Logger(string logFileName)
    {
      this.logFileName = logFileName;
      InitiateLog();
    }

    public void LogInfo(string message, params object[] parameters)
    {
      this.Log("Info", message, parameters);
    }

    public void LogError(string message, params object[] parameters)
    {
      this.Log("Error", message, parameters);
    }

    private void InitiateLog()
    {
      if (!File.Exists(logFileName))
      {
        File.Create(logFileName).Close();
      }

      LogInfo("Start logging all timestamps are in UTC.");
    }

    private void Log(string logLevel, string message, params object[] parameters)
    {
      string[] status =
      {
        string.Format("{0} - {1}: {2}",
          DateTime.UtcNow,
          logLevel,
          string.Format(message, parameters))
      };
      File.AppendAllLines(logFileName, status);
      Console.WriteLine(status[0]);
    }
  }
}
