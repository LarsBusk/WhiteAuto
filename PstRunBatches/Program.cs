using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Common.Ui;
using Common.Utilities;
using PstRunBatches.Properties;

namespace PstRunBatches
{
  class Program
  {
    private static string[] batchNames = new[] { "Specification7", "Specification13", "Specification5", "Specification20", "Specification10" };
    private static int batchNumber = 0;
    private static MantaFunctions pstHelpers;
    static void Main(string[] args)
    {
      pstHelpers = new MantaFunctions();

      SelectNextBatch();

      int i = 1;

      while (true)
      {
        pstHelpers.ClickStartStopButton();

        UpdateDisp(i);

        pstHelpers.CloseBatch();

        SelectNextBatch();

        i++;
      }
    }

    private static void UpdateDisp(int i)
    {
      int tid = Settings.Default.MinutesBetweenNewBatch;
      while (tid > 0)
      {
        Console.Clear();
        Console.WriteLine($"Batch no {i} running {tid} min missing.");
        Thread.Sleep(TimeSpan.FromSeconds(60));
        tid--;
      }
    }

    private static void SelectNextBatch()
    {
      if (batchNumber >= batchNames.Length)
      {
        batchNumber = 0;
      }

      pstHelpers.SelectBatch(batchNames[batchNumber]);
      batchNumber++;
    }
  }
}
