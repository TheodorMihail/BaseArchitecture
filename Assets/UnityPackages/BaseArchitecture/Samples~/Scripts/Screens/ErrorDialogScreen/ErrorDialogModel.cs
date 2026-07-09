
using static BaseArchitecture.Core.Screens.ErrorDialogScreen;

namespace BaseArchitecture.Core.Screens
{
    public class ErrorDialogModel : Model, IModelWithParams<ErrorDialogScreenParams>
    {
        public string Message { get; set; }

        public void InitializeWithParameters(ErrorDialogScreenParams parameters)
        {
            Message = parameters.Message ?? "Unknown error";
        }
    }
}
