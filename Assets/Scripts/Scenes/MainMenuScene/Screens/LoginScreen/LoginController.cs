using Base.Project;
using Base.Systems;
using static Base.Scenes.MainMenu.LoginScreen;

namespace Base.Scenes.MainMenu
{
    public class LoginController : Controller<LoginScreen, LoginModel, LoginView, LoginScreenResult>
    {
        private IAuthenticationManager _authenticationManager;

        public LoginController(LoginScreen loginScreen, LoginModel model, LoginView view,
            IAuthenticationManager authenticationManager)
            : base(loginScreen, model, view)
        {
            _authenticationManager = authenticationManager;
        }

        public override void Initialize()
        {
            base.Initialize();
            _view.OnLoginConfirmBtnPressed += OnLoginConfirmBtnPressed;
            _view.OnRegisterBtnPressed += OnRegisterBtnPressed;
            _view.ShowLoginPanel();
        }

        public override void Dispose()
        {
            _view.OnLoginConfirmBtnPressed -= OnLoginConfirmBtnPressed;
            _view.OnRegisterBtnPressed -= OnRegisterBtnPressed;
            base.Dispose();
        }

        private void OnRegisterBtnPressed()
        {
            CloseScreen(new LoginScreenResult() { State = ResultType.Register });
        }

        private async void OnLoginConfirmBtnPressed((string username, string password) credentials)
        {
            _view.ShowLoadingPanel();
            bool loginSuccess = await _authenticationManager.LoginUser(credentials);

            if (loginSuccess)
            {
                CloseScreen(new LoginScreenResult() { State = ResultType.LoginSuccess });
            }
            else
            {
                _view.ShowLoginPanel();
            }
        }
    }
}