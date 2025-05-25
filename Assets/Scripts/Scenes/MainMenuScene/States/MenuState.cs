using Base.Project;
using Base.Systems;
using static Base.Scenes.MainMenu.MenuScreen;
using static Base.Scenes.MainMenu.MainMenuStateMachine;

namespace Base.Scenes.MainMenu
{
    public class MenuState : BaseState<MainMenuStateIds>
    {
        public override MainMenuStateIds Id => MainMenuStateIds.Menu;

        private readonly IUIManager _uiManager;

        public MenuState(IUIManager uiManager)
        {
            _uiManager = uiManager;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            ShowMenuScreen();
        }

        private async void ShowMenuScreen()
        {
            var menuScreenResult = await _uiManager.ShowScreen<MenuScreen, MenuScreenResult>();
            FinishState(menuScreenResult);
        }
    }
}