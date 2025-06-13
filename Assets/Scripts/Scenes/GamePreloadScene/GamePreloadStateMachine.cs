using Base.Project;
using Base.Systems;
using System.Collections.Generic;
using static Base.Scenes.GamePreload.GamePreloadStateMachine;

namespace Base.Scenes.GamePreload
{
    public class GamePreloadStateMachine : BaseStateMachine<GamePreloadStateIds>
    {
        public enum GamePreloadStateIds
        {
            SplashState,
            BootState
        }

        protected override GamePreloadStateIds DefaultStateId => GamePreloadStateIds.SplashState;

        private IScenesManager _scenesManager;

        public GamePreloadStateMachine(IList<IState<GamePreloadStateIds>> preloadStates,
            IScenesManager scenesManager) : base(preloadStates)
        {
            _scenesManager = scenesManager;
        }

        protected override void OnStateFinished((GamePreloadStateIds stateId, object[] paramsList) finishedState)
        {
            switch (finishedState.stateId)
            {
                case GamePreloadStateIds.SplashState:
                    SetState(GamePreloadStateIds.BootState);
                    break;
                case GamePreloadStateIds.BootState:
                    _scenesManager.LoadScene(ScenesManager.SceneType.MainMenu.ToString());
                    break;
            }
        }
    }
}