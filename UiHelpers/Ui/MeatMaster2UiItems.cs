using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestStack.White.UIItems.Finders;

namespace Common.Ui
{
  public class MeatMaster2UiItems
  {
    #region Main app items

    public const string Mm2MainWindowName = "MainWindow";

    public const string Mm2ProcessName = "FOSS.Nova.UI.Antonius.Core";

    public const string MenuBarApplicationSwitchButton = "MenuBarApplicationSwitchButton";

    public const string PathToMeatMaster = @"C:\Program Files (x86)\FOSS\MeatMaster II\FOSS.Nova.UI.Antonius.Core.exe";

    #endregion Main app items

    #region CareView Items

    public static SearchCriteria CareViewButton = SearchCriteria.ByText("MenuBarCareButton");

    public const string CareViewInstrumentDiagnosticsButton = "CareViewInstrumentDiagnosticsButton";

    public const string CareViewExportLogsButton = "CareViewExportLogsButton";

    public const string CloseDownButton = "CareViewShutDownButton";

    public const string ExportLogsDialog = "ExportLogsPopup";

    public const string PopupCollectLogsButton = "PopupCollectLogsButton";

    public const string ProgressPopup = "ProgressPopup";

    public const string PopupCloseButton = "PopupCloseButton";

    #endregion

    public static SearchCriteria Mm2StartStopButton = SearchCriteria.ByText("MenuBarStartStopButton");

    public const string InstrumentStartupCancelButton = "PopupCancelButton";

    public const string MessageBoxPopup = "MessageBoxPopup";

    public const string PopupLeftButton = "PopupLeftButton";

    public const string PopupRightButton = "PopupRightButton";

    public const string InstrumentStartupStartButton = "InstrumentStartupStartButton";

    public const string InstrumentDiagnosticDialog = "InstrumentDiagnosticsPopup";

    public const string InstrumentDiagnosticRepeatButton = "ButtonPopUpRepeat";

    public const string InstrumentDiagnosticOkButton = "PopupOkButton";

    public const string InstrumentDiagnosticCancelButton = "PopupCancelButton";

    public const string CloseDownDialog = "MessageBoxPopup";

    public const string PopupOkButton = "PopupLeftButton";

    public const string SplashPopup = "SplashPopup";

    #region OnTopView Items

    public const string OnTopProcessName = "FOSS.Nova.UI.OnTopManager.Core";

    public const string OnTopWindowName = "OnTopView";

    #endregion
  }
}
