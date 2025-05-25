using Base.Project;
using Base.Systems;
using System.Collections.Generic;
using static Base.Scenes.MainMenu.MainMenuStateMachine;

namespace Base.Scenes.MainMenu
{
    public class MainMenuStateMachine : BaseStateMachine<MainMenuStateIds>
    {
        public enum MainMenuStateIds
        {
            Authentication,
            Characters
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
                    SetState(MainMenuStateIds.Characters);
                    break;
                case MainMenuStateIds.Characters:
                    if(finishedState.paramsList.Length == 0)
                    {
                        SetState(MainMenuStateIds.Authentication);
                    }
                    else
                    {
                        _scenesManager.LoadScene(ScenesManager.SceneType.Game.ToString());
                    }
                    break;
            }
        }
    }
}