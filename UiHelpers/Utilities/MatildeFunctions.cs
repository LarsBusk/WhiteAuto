using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Common.Ui;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;

namespace Common.Utilities
{
  public class MatildeFunctions : Functions
  {
    public MatildeFunctions() :
      base(Uis.Matilde){}

    public void ClickMotorDriftButton()
    {
      try
      {
        Button StartMotorDriftButton =
          MainWindow.Get<Button>(SearchCriteria.ByText(UiItems.ServiceViewStartMotorDriftTestButton));
        while (!StartMotorDriftButton.Enabled)
        {
          Thread.Sleep(TimeSpan.FromSeconds(10));
        }

        StartMotorDriftButton.Click();

        Label stateLabel = MainWindow.Get<Label>(SearchCriteria.ByAutomationId(UiItems.MenuBarInstrumentStateText));

        while (stateLabel.Name != "TestingMotorDrift")
        {
          Thread.Sleep(TimeSpan.FromSeconds(10));
        }
      }
      catch (Exception e)
      {
        logger.LogError(e.Message);
      }
    }
  }
}
