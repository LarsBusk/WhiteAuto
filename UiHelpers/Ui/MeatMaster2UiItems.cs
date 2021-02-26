using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestStack.White.UIItems.Finders;

namespace Common.Ui
{
  public class MeatMaster2UiItems
  {
    public const string Mm2MainWindowName = "MainWindow";

    public static SearchCriteria Mm2StartStopButton = SearchCriteria.ByText("MenuBarStartStopButton");

    public const string Mm2ProcessName = "FOSS.Nova.UI.Antonius.Core";

    public const string InstrumentStartupCancelButton = "PopupCancelButton";

    public static SearchCriteria CareViewButton = SearchCriteria.ByText("MenuBarCareButton");

    public static SearchCriteria CareViewInstrumentDiagnosticsButton = SearchCriteria.ByText("CareViewInstrumentDiagnosticsButton");

    public static SearchCriteria CareViewExportLogsButton = SearchCriteria.ByText("CareViewExportLogsButton");

    public const string CloseDownButton = "CareViewShutDownButton";

    public const string MessageBoxPopup = "MessageBoxPopup";

    public const string PopupLeftButton = "PopupLeftButton";

    public const string PopupRightButton = "PopupRightButton";

    public const string InstrumentStartupStartButton = "InstrumentStartupStartButton";

    public const string InstrumentDiagnosticDialog = "InstrumentDiagnosticsPopup";

    public const string ExportLogsDialog = "ExportLogsPopup";

    public const string PopupCollectLogsButton = "PopupCollectLogsButton";

    public const string InstrumentDiagnosticRepeatButton = "ButtonPopUpRepeat";

    public const string InstrumentDiagnosticOkButton = "PopupOkButton";

    public const string InstrumentDiagnosticCancelButton = "PopupCancelButton";

    public const string MenuBarApplicationSwitchButton = "MenuBarApplicationSwitchButton";

    public const string CloseDownDialog = "MessageBoxPopup";

    public const string PopupOkButton = "PopupLeftButton";

    public const string SplashPopup = "SplashPopup";

    #region OnTopView Items

    public const string OnTopProcessName = "FOSS.Nova.UI.OnTopManager.Core";

    public const string OnTopWindowName = "OnTopView";

    #endregion
  }
}
