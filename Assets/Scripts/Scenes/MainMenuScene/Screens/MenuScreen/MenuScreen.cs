using Base.Systems;
using static Base.Scenes.MainMenu.MenuScreen;

namespace Base.Scenes.MainMenu
{
    public class MenuScreen : Screen<MenuModel, MenuView, MenuController>, IScreenWithResult<MenuScreenResult>
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

        private MenuScreenResult _result;
        public MenuScreenResult GetResult() => _result;
        public void SetResult(MenuScreenResult result) => _result = result;
    }
}