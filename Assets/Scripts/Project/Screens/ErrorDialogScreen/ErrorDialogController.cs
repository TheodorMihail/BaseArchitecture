using Base.Systems;

namespace Base.Project
{
    public class ErrorDialogController : Controller<ErrorDialogScreen, ErrorDialogModel, ErrorDialogView>
    {
        public ErrorDialogController(ErrorDialogScreen screen, ErrorDialogModel model, ErrorDialogView view)
            : base(screen, model, view)
        {
        }

        public override void Initialize()
        {
            _view.OnConfirmPressed += OnConfirmPressed;
            _view.SetMessage(_model.Message);
            base.Initialize();
        }

        public override void Dispose()
        {
            _view.OnConfirmPressed -= OnConfirmPressed;
            base.Dispose();
        }

        private void OnConfirmPressed()
        {
            CloseScreen();
        }
    }
}
