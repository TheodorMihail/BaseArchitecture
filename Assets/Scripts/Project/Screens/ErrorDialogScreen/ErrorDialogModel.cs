using Base.Systems;
using static Base.Project.ErrorDialogScreen;

namespace Base.Project
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
