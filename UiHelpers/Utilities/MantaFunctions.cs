using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using Common.Ui;
using TestStack.White.UIItems;
using TestStack.White.UIItems.ListViewItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;

namespace Common.Utilities
{
  public class MantaFunctions : Functions
  {
    #region private properties

    private string SelectBatchDialogName { get; set; }

    private SearchCriteria BatchList { get; set; }

    #endregion

    #region Public properties
    
    #endregion

    #region public methods

    public MantaFunctions()
      :base(Uis.ProcessTouch)
    { 
      logger.LogInfo("Creating a new instance of MantaFunctions.");

      this.MainWindow = ApplicationHelpers.GetMainWindow(UiItems.MantaProcessname, UiItems.MantaWindowtitle);
      this.StartStopButton = UiItems.MantaStartStopButton;
    }

    /// <summary>
    /// Envoked to close the current batch in Manta
    /// </summary>
    public void CloseBatch()
    {
      ClickButton(SearchCriteria.ByText(UiItems.MantaLolipopButtonName));

      while (!WaitHelpers.WaitFor(() => MainWindow.ModalWindows().Any(), TimeSpan.FromMinutes(2)))
      {
        ClickButton(SearchCriteria.ByText(UiItems.MantaLolipopButtonName));
      }

      //WaitHelpers.WaitForDialog(MainWindow, "", TimeSpan.FromSeconds(60));
      Window batchControlDialog = MainWindow.ModalWindows()[0];

      ClickButton(SearchCriteria.ByText("BatchDialogCloseBatchButton"), batchControlDialog);

      Thread.Sleep(TimeSpan.FromSeconds(10));
    }

    public void SelectBatch(string batchName)
    {
      ClickButton(SearchCriteria.ByText(UiItems.MantaSelectBatchButton));
      
      ListView batchListView = MainWindow.Get<ListView>(SearchCriteria.ByAutomationId("FormulaList"));
      logger.LogInfo($"ListView {batchListView.Name} found.");

      SelectBatchFromList(batchName, batchListView);

      ClickButton(SearchCriteria.ByText(UiItems.RecepieSelectButton));
    }

    #endregion

    private void SelectBatchFromList(string batchName, ListView listview)
    {
      logger.LogInfo($"Trying to select batch {batchName}");
      ListViewRows rows = listview.Rows;

      /*foreach (var listViewRow in rows)
      {
        logger.LogInfo($"Row found {listViewRow.Cells[0].Name}");
      }*/

      ListViewRow row = rows.First(r => r.Cells[0].Name.Equals(batchName));
      logger.LogInfo($"Selecting Batch with name: {row.Cells[0].Name}");
      row.Select();
    }
  }
}
