using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Ui;
using Common.Utilities;
using TestStack.White.UIItems;
using TestStack.White.UIItems.WindowItems;

namespace AdvUiDiagnostics
{
  class Program
  {
    static void Main(string[] args)
    {
      Functions helper = new Functions(Uis.Advanced);

      while (true)
      {
        helper.ClickButton(UiItems.CareViewInstrumentDiagnosticsButton);

        helper.HandlePopup("yes");


        WaitHelpers.WaitForDialog(helper.MainWindow, UiItems.InstrumentDiagnosticsPopupName, TimeSpan.FromSeconds(30));
        Window diagDialog = helper.MainWindow.ModalWindow(UiItems.InstrumentDiagnosticsPopupName);
        Button okButton = diagDialog.Get<Button>(UiItems.PopupOkButton);
        WaitHelpers.WaitForEnabled(okButton, TimeSpan.FromMinutes(20), TimeSpan.FromSeconds(20));

        okButton.Click();

        WaitHelpers.WaitSeconds(30);
      }
    }
  }
}
