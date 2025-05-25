using Base.Project;
using Base.Systems;
using System.Collections.Generic;
using static Base.Scenes.Preload.PreloadStateMachine;

namespace Base.Scenes.Preload
{
    public class PreloadStateMachine : BaseStateMachine<PreloadStateIds>
    {
        public enum PreloadStateIds
        {
            SplashState,
            BootState
        }

        protected override PreloadStateIds DefaultStateId => PreloadStateIds.SplashState;

        private IScenesManager _scenesManager;

        public PreloadStateMachine(IList<IState<PreloadStateIds>> preloadStates,
            IScenesManager scenesManager) : base(preloadStates)
        {
            _scenesManager = scenesManager;
        }

        protected override void OnStateFinished((PreloadStateIds stateId, object[] paramsList) finishedState)
        {
            switch (finishedState.stateId)
            {
                case PreloadStateIds.SplashState:
                    SetState(PreloadStateIds.BootState);
                    break;
                case PreloadStateIds.BootState:
                    _scenesManager.LoadScene(ScenesManager.SceneType.MainMenu.ToString());
                    break;
            }
        }
    }
}