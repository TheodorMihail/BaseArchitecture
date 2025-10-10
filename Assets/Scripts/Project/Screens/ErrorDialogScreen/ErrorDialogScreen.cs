using Base.Systems;
using Zenject;

namespace Base.Project
{
    public class ErrorDialogScreen : Screen<ErrorDialogModel, ErrorDialogView, ErrorDialogController>
    {
        [Inject] private readonly object[] _parameters;

        protected override void MVCCreated()
        {
            base.MVCCreated();
            _model.Message = _parameters[0] as string;
        }
    }
}
