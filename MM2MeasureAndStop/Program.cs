﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Ui;
using Common.Utilities;

namespace MM2MeasureAndStop
{
  class Program
  {
    static void Main(string[] args)
    {
      MeatMaster2Functions functions = new MeatMaster2Functions();

      Random random = new Random();

      //int.Parse(Enumerable.FirstOrDefault<string>((IEnumerable<string>) args) ?? "120");
      while (true)
      {
        int secondsToWait = 3600 + random.Next(7200);
        functions.ClickStartStopButton();
        WaitHelpers.WaitSeconds(secondsToWait);
      }
    }
  }
}
