using Base.Project;
using Base.Systems;
using static Base.Scenes.MainMenu.RegisterScreen;

namespace Base.Scenes.MainMenu
{
    public class RegisterController : Controller<RegisterScreen, RegisterModel, RegisterView, RegisterScreenResult>
    {
        private IAuthenticationManager _authenticationManager;

        public RegisterController(RegisterScreen RegisterScreen, RegisterModel model, RegisterView view,
            IAuthenticationManager authenticationManager)
            : base(RegisterScreen, model, view)
        {
            _authenticationManager = authenticationManager;
        }

        public override void Initialize()
        {
            base.Initialize();
            _view.OnRegisterConfirmBtnPressed += OnRegisterConfirmBtnPressed;
            _view.OnRegisterCancelBtnPressed += OnRegisterCancelBtnPressed;
            _view.ShowRegisterPanel();
        }

        public override void Dispose()
        {
            _view.OnRegisterConfirmBtnPressed -= OnRegisterConfirmBtnPressed;
            _view.OnRegisterCancelBtnPressed -= OnRegisterCancelBtnPressed;
            base.Dispose();
        }

        private async void OnRegisterConfirmBtnPressed((string username, string password) credentials)
        {
            _view.ShowLoadingPanel();
            bool registerSuccess = await _authenticationManager.RegisterUser(credentials);

            if (registerSuccess)
            {
                CloseScreen(new RegisterScreenResult() { State = ResultType.RegisterSuccess });
            }
            else
            {
                _view.ShowRegisterPanel();
            }
        }

        private void OnRegisterCancelBtnPressed()
        {
            CloseScreen(new RegisterScreenResult() { State = ResultType.RegisterCanceled });
        }
    }
}