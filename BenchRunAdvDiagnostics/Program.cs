using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.Ui;
using Common.Utilities;

namespace BenchRunAdvDiagnostics
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Functions functions = new Functions(Uis.Advanced);

            var timesToRun = Properties.Settings.Default.RepeatNumber ==
                0 ? int.MaxValue : Properties.Settings.Default.RepeatNumber;

            while (timesToRun > 0)
            {
                if (Properties.Settings.Default.RunDetecorTest) functions.SelectAndStartTest("Detector module");
                if (Properties.Settings.Default.RunMonoTest) functions.SelectAndStartTest("Monochromator module");
                if (Properties.Settings.Default.RunSamplemotorTest) functions.SelectAndStartTest("Sample motor module");
                if (Properties.Settings.Default.RunAllTests) functions.SelectAndRunAllTests();
                timesToRun--;
            }

        }
    }
}
