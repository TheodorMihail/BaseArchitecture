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
    
        protected override void MVCCreated()
        {
            base.MVCCreated();
            _model.Message = _parameter.Message ?? "Unknown error";
        }
    }
}
