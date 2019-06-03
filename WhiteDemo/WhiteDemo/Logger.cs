using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhiteDemo
{
  public class Logger
  {
    private const string LogFileName = "WhiteTest.log";

    public Logger()
    {
      if (!File.Exists(LogFileName))
      {
        File.Create(LogFileName);
        Log($"Logfile {LogFileName} is created.");
      }

    }
    public void Log(string message)
    {
      Console.WriteLine(message);
      File.AppendAllText(LogFileName, $"{message}\n");
    }
  }
}
