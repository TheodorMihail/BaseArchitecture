using Base.Systems;
using static Base.Scenes.MainMenu.MenuScreen;

namespace Base.Scenes.MainMenu
{
    public class MenuScreen : Screen<MenuModel, MenuView, MenuController, MenuScreenResult>
    {
        public enum ResultType
        {
            MapSelected,
            MapSelectionCanceled
        }

        public struct MenuScreenResult : IScreenResult
        {
            public string MapUID;
            public ResultType State;
        }
    }
}