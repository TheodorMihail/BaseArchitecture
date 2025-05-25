using Base.Project;
using Base.Systems;
using static Base.Scenes.Preload.PreloadStateMachine;

namespace Base.Scenes.Preload
{
    public class SplashState : BaseState<PreloadStateIds>
    {
        public override PreloadStateIds Id => PreloadStateIds.SplashState;

        private readonly IUIManager _uiManager;

        public SplashState(IUIManager uiManager)
        {
            _uiManager = uiManager;
        }

        public override async void OnEnter()
        {
            base.OnEnter();

            await _uiManager.ShowScreen<SplashScreen>();
            FinishState();
        }
    }
}