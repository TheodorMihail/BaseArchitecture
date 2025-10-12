using Base.Systems;
using static Base.Scenes.MainMenu.RegisterScreen;

namespace Base.Scenes.MainMenu
{
    public class RegisterScreen : Screen<RegisterModel, RegisterView, RegisterController> , IScreenWithResult<RegisterScreenResult>
    {
        public enum ResultType
        {
            RegisterCanceled,
            RegisterSuccess
        }

        public struct RegisterScreenResult : IScreenResult
        {
            public ResultType State { get; set; }
        }

        private RegisterScreenResult _result;
        public RegisterScreenResult GetResult() => _result;
        public void SetResult(RegisterScreenResult result) => _result = result;
    }
}