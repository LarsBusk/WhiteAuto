using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Common.Utilities;

namespace FiChangeMode
{
  class Program
  {
    static void Main(string[] args)
    {
      FiFunctions helpers = new FiFunctions();

      while (true)
      {
        helpers.ChangeMode(FiFunctions.FiMode.Measuring);
        
        Thread.Sleep(TimeSpan.FromHours(1));

        helpers.ChangeMode(FiFunctions.FiMode.Standby);

        Thread.Sleep(TimeSpan.FromMinutes(3));

        helpers.ChangeMode(FiFunctions.FiMode.Stop, true);

        Thread.Sleep(TimeSpan.FromMinutes(3));

        helpers.ChangeMode(FiFunctions.FiMode.Standby);

        Thread.Sleep(TimeSpan.FromMinutes(3));
      }
    }
  }
}
