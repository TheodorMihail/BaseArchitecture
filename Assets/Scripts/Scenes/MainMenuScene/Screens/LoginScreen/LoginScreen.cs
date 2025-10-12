using Base.Systems;
using static Base.Scenes.MainMenu.LoginScreen;

namespace Base.Scenes.MainMenu
{
    public class LoginScreen : Screen<LoginModel, LoginView, LoginController>, IScreenWithResult<LoginScreenResult>
    {
        public enum ResultType
        {
            Register,
            LoginSuccess
        }
    
        public struct LoginScreenResult : IScreenResult
        {
            public ResultType State { get; set; }
        }

        private LoginScreenResult _result;

        public LoginScreenResult GetResult() => _result;
        public void SetResult(LoginScreenResult result) => _result = result;
    
    }
}