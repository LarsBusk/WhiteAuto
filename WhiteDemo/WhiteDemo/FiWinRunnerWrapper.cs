using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;
using Castle.Core.Internal;
using TestStack.White;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;

namespace WhiteDemo
{
  public class FiWinRunnerWrapper
  {
    private const string fiWinrunnerDll = @"C:\Program Files\FossIntegrator\2x1x0\FI_Winrunner.dll";

    private Logger logger;

    #region Job view functions

    [DllImport(fiWinrunnerDll, SetLastError = true)]
    private static extern int JV_GetJobCount(int hWnd, out int piCount);

    [DllImport(fiWinrunnerDll, SetLastError = true)]
    private static extern int JV_GetComponentCount(int hWnd, int jobNumber, out int componentCount);

    [DllImport(fiWinrunnerDll, SetLastError = true)]
    private static extern int JV_GetComponentText(int hWnd, int jobNumber, int componentNumber, ref char[] componentText);


    [DllImport(fiWinrunnerDll, SetLastError = true)]
    private static extern int JV_GetComponentValue(int hWnd, int jobNumber, int componentNumber, out string componentValue);

    #endregion

    #region Result view functions

    [DllImport(fiWinrunnerDll, SetLastError = true)]
    private static extern int RV_GetJobCount(int hWnd, out int pisCount);

    [DllImport(fiWinrunnerDll, SetLastError = true)]
    private static extern int RV_GetJobName(int hWnd, int jobNumber, out string jobName);

    [DllImport(fiWinrunnerDll, SetLastError = true)]
    private static extern int RV_GetJobType(int hWnd, int jobNumber, out string jobType);

    [DllImport(fiWinrunnerDll, SetLastError = true)]
    private static extern int RV_GetComponentCount(int hWnd, int jobNumber, out int componentCount);

    [DllImport(fiWinrunnerDll, SetLastError = true)]
    private static extern int RV_GetComponentText(int hWnd, int jobNumber, int componentNumber, out string componentText);

    [DllImport(fiWinrunnerDll, SetLastError = true)]
    private static extern int RV_GetSampleCount(int hWnd, int jobNumber, out int sampleCount);

    [DllImport(fiWinrunnerDll, SetLastError = true)]
    private static extern int RV_GetComponentValue(int hWnd, int jobNumber, int sampleNumber, int componentNumber, out string componentValue);

    #endregion

    public FiWinRunnerWrapper()
    {
      logger = new Logger();
    }

    public void GetStuff()
    {
      Application fi = Application.Attach(Process.GetProcessesByName("WSF").FirstOrDefault());
      logger.Log($"Program found {fi.Name}");
      List<Window> fiWindows = fi.GetWindows();

      Window mainWindow = fiWindows.FirstOrDefault(w => w.Title.StartsWith("Foss Integrator"));

      List<Window> modals = mainWindow.ModalWindows();

      Window jobResultDisp = modals.Find(w => w.Title.Equals("Result/Job Display"));

      var jobPanels = jobResultDisp.Items;

      int jobWindowPointer =
        (int) jobPanels.Find(p => p.Id.Equals("59660"))
          .AutomationElement.GetCurrentPropertyValue(AutomationElement.NativeWindowHandleProperty);

    Panel resultView =
        ((Panel) jobPanels.Find(p => p.Id.Equals("1617"))).Get<Panel>(SearchCriteria.ByAutomationId("59649"));
      int resWindowPointer =
        (int) resultView.AutomationElement.GetCurrentPropertyValue(AutomationElement.NativeWindowHandleProperty);

      int noOfJobsInJobView = 0;
      int noOfJobsInResView = 0;
      int componentCount = 0;

      JV_GetJobCount(jobWindowPointer, out noOfJobsInJobView);
      RV_GetJobCount(resWindowPointer, out noOfJobsInResView);

      logger.Log($"{noOfJobsInResView} found in the result view.");
      logger.Log($"{noOfJobsInJobView} found in the job view.");

      for (int i = 1; i < noOfJobsInResView; i++)
      {
        var jobName = string.Empty;
         RV_GetJobName(resWindowPointer, i, out jobName);
         logger.Log($"Job nr {i} has name: {jobName}");
       }

      for (int jobnr = 0; jobnr < noOfJobsInJobView; jobnr++)
      {
        int compCount =  0;
        JV_GetComponentCount(jobWindowPointer, jobnr, out compCount);
        logger.Log($"Job no {jobnr} has {compCount} components.");
        var text = string.Empty; // new char[4096];
        int res = 0;
        //res = JV_GetComponentText(jobWindowPointer, jobnr, 1, ref text);
        logger.Log($"Result of {jobnr} is {res}. Text is null {text.IsNullOrEmpty()}");

        logger.Log($"Job no {jobnr} has component text {text}");
        /*for (int compnr = 0; compnr < compCount; compnr++)
        {
          //char* compText;
          var text =  new char[256];
          //string componentText = text.ToString();
          res = JV_GetComponentText(jobWindowPointer, jobnr, compnr, ref text);
          Logger.Log($"Result of {jobnr}, {compnr} is {res}. Text is null {text.IsNullOrEmpty()}");
          if (text != null)
          {
            //string componentText = new string(text);
            Logger.Log($"Component nr {compnr} in job nr: {jobnr} has name  {text}");
          }
        }*/

      }

      var status = new char[4096];
      JV_GetComponentText(jobWindowPointer, 2, 1, ref status);
      logger.Log($"jobnr {noOfJobsInJobView} has status {status}");

    }
  }
}
