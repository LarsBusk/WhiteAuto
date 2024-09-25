using Common.Ui;
using Common.Utilities;
using System;
using TestStack.White.UIItems;
using TestStack.White.UIItems.WindowItems;

namespace InstrumentDiagnostics
{
    class Program
  {
    static void Main(string[] args)
    {

        var myUi = new Uis();
        if (Uis.TryParse(args[0], out myUi))
        {


            Functions helper = new Functions(myUi);

            while (true)
            {
                helper.ClickButton(UiItems.CareViewInstrumentDiagnosticsButton);

                helper.HandlePopup("yes");


                WaitHelpers.WaitForDialog(helper.MainWindow, UiItems.InstrumentDiagnosticsPopupName,
                    TimeSpan.FromSeconds(30));
                Window diagDialog = helper.MainWindow.ModalWindow(UiItems.InstrumentDiagnosticsPopupName);
                Button okButton = diagDialog.Get<Button>(UiItems.PopupOkButton);
                WaitHelpers.WaitForEnabled(okButton, TimeSpan.FromMinutes(20), TimeSpan.FromSeconds(20));

                okButton.Click();

                WaitHelpers.WaitSeconds(30);
            }
        }
    }
  }
}
