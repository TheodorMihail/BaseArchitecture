using Base.Systems;
using static Base.Scenes.MainMenu.MenuScreen;

namespace Base.Scenes.MainMenu
{
    public class MenuScreen : ScreenWithResult<MenuModel, MenuView, MenuController, MenuScreenResult>
    {
        public enum ResultType
        {
            MapSelected,
            MapSelectionCanceled
        }

        public struct MenuScreenResult : IScreenResult
        {
            public string MapUID { get; set; }
            public ResultType State { get; set; }
        }
    }
}