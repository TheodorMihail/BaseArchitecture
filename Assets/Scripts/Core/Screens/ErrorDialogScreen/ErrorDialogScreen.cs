using static BaseArchitecture.Core.Screens.ErrorDialogScreen;

namespace BaseArchitecture.Core.Screens
{
    public class ErrorDialogScreen : ScreenWithParams<ErrorDialogModel, ErrorDialogView, ErrorDialogController, ErrorDialogScreenParams>
    {
        public struct ErrorDialogScreenParams : IScreenParam
        {
            public string Message { get; set; }
        }
    }
}
