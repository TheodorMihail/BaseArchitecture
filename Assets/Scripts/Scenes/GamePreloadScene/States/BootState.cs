using Base.Project;
using Base.Systems;
using static Base.Scenes.GamePreload.GamePreloadStateMachine;

namespace Base.Scenes.GamePreload
{
    public class BootState : BaseState<GamePreloadStateIds>
    {
        public override GamePreloadStateIds Id => GamePreloadStateIds.BootState;

        private readonly IUIManager _uiManager;

        public BootState(IUIManager uiManager)
        {
            _uiManager = uiManager;
        }

        public override async void OnEnter()
        {
            base.OnEnter();

            await _uiManager.ShowScreen<BootScreen>();
            FinishState();
        }
    }
}