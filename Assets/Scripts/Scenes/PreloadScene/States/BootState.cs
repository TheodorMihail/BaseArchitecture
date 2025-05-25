using Base.Project;
using Base.Systems;
using static Base.Scenes.Preload.PreloadStateMachine;

namespace Base.Scenes.Preload
{
    public class BootState : BaseState<PreloadStateIds>
    {
        public override PreloadStateIds Id => PreloadStateIds.BootState;

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