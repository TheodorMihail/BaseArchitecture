using Base.Systems;
using static Base.Scenes.MainMenu.RegisterScreen;

namespace Base.Scenes.MainMenu
{
    public class RegisterScreen : ScreenWithResult<RegisterModel, RegisterView, RegisterController, RegisterScreenResult>
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
    }
}