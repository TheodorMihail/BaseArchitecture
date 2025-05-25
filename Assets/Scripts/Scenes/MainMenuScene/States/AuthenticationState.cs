using Base.Project;
using Base.Systems;
using static Base.Scenes.MainMenu.LoginScreen;
using static Base.Scenes.MainMenu.MainMenuStateMachine;
using static Base.Scenes.MainMenu.RegisterScreen;

namespace Base.Scenes.MainMenu
{
    public class AuthenticationState : BaseState<MainMenuStateIds>
    {
        public override MainMenuStateIds Id => MainMenuStateIds.Authentication;

        private readonly IUIManager _uiManager;

        public AuthenticationState(IUIManager uiManager)
        {
            _uiManager = uiManager;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            ShowLoginScreen();
        }

        private async void ShowLoginScreen()
        {
            var loginScreenResult = await _uiManager.ShowScreen<LoginScreen, LoginScreenResult>();
            if (loginScreenResult.State == LoginScreen.ResultType.LoginSuccess)
            {
                FinishState();
            }
            else
            {
                ShowRegisterScreen();
            }
        }

        private async void ShowRegisterScreen()
        {
            var registerScreenResult = await _uiManager.ShowScreen<RegisterScreen, RegisterScreenResult>();
            if (registerScreenResult.State == RegisterScreen.ResultType.RegisterSuccess)
            {
                FinishState();
            }
            else
            {
                ShowLoginScreen();
            }
        }
    }
}