using Base.Systems;
using static Base.Scenes.MainMenu.LoginScreen;

namespace Base.Scenes.MainMenu
{
    public class LoginScreen : Screen<LoginModel, LoginView, LoginController, LoginScreenResult>
    {
        public enum ResultType
        {
            Register,
            LoginSuccess
        }

        public struct LoginScreenResult : IScreenResult
        {
            public ResultType State;
        }
    }
}