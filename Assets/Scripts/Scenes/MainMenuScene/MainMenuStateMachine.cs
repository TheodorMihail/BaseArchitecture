using Base.Project;
using Base.Systems;
using System.Collections.Generic;
using static Base.Scenes.MainMenu.MainMenuStateMachine;
using static Base.Scenes.MainMenu.MenuScreen;

namespace Base.Scenes.MainMenu
{
    public class MainMenuStateMachine : BaseStateMachine<MainMenuStateIds>
    {
        public enum MainMenuStateIds
        {
            Authentication,
            Menu
        }

        protected override MainMenuStateIds DefaultStateId => MainMenuStateIds.Authentication;

        private IScenesManager _scenesManager;

        public MainMenuStateMachine(IList<IState<MainMenuStateIds>> mainMenuStates,
            IScenesManager scenesManager) : base(mainMenuStates)
        {
            _scenesManager = scenesManager;
        }

        protected override void OnStateFinished((MainMenuStateIds stateId, object[] paramsList) finishedState)
        {
            switch (finishedState.stateId)
            {
                case MainMenuStateIds.Authentication:
                    SetState(MainMenuStateIds.Menu);
                    break;
                case MainMenuStateIds.Menu:
                    MenuScreenResult result = (MenuScreenResult)finishedState.paramsList[0];
                    
                    if(result.State == ResultType.MapSelectionCanceled)
                    {
                        SetState(MainMenuStateIds.Authentication);
                        return;
                    }

                    _scenesManager.LoadScene(ScenesManager.SceneType.Game.ToString());

                    break;
            }
        }
    }
}