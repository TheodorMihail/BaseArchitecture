using Base.Systems;
using static Base.Project.ErrorDialogScreen;

namespace Base.Project
{
    public class ErrorDialogScreen : ScreenWithParams<ErrorDialogModel, ErrorDialogView, ErrorDialogController, ErrorDialogScreenParams>
    {
        public struct ErrorDialogScreenParams : IScreenParam
        {
            public string Message { get; set; }
        }
    }
}
