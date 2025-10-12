using Base.Systems;
using static Base.Project.ErrorDialogScreen;

namespace Base.Project
{
    public class ErrorDialogScreen : Screen<ErrorDialogModel, ErrorDialogView, ErrorDialogController>, IScreenWithParams<ErrorDialogScreenParams>
    {
        public struct ErrorDialogScreenParams : IScreenParam
        {
            public string Message { get; set; }
        }

        private ErrorDialogScreenParams _parameter;

        public ErrorDialogScreenParams GetParameter() => _parameter;

        public void SetParameter(ErrorDialogScreenParams parameter) => _parameter = parameter;

        protected override void MVCCreated()
        {
            base.MVCCreated();

            // Automatically initialize model with parameters if it supports them
            if (_model is IModelWithParams<ErrorDialogScreenParams> modelWithParams)
            {
                modelWithParams.InitializeWithParameters(_parameter);
            }
        }
    }
}
