using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Ui;
using Common.Utilities;

namespace PfMeasureAndStop
{
  class Program
  {
    static void Main(string[] args)
    {
      Functions functions = new Functions(Uis.pNova);

      int secondsToWait = int.Parse(Enumerable.FirstOrDefault<string>((IEnumerable<string>) args) ?? "120");
      while (true)
      {
        functions.ClickStartStopButton();
        WaitHelpers.WaitSeconds(secondsToWait);
      }
    }
  }
}
