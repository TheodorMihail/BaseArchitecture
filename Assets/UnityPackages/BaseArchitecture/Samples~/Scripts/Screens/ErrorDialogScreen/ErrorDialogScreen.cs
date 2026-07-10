using static BaseArchitecture.Core.Screens.ErrorDialogScreen;

namespace BaseArchitecture.Core.Screens
{
    public class ErrorDialogScreen : Screen<ErrorDialogModel, ErrorDialogView, ErrorDialogController>
    {
        public struct ErrorDialogScreenParams
        {
            public string Message { get; set; }
        }
    }
}
