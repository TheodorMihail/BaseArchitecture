using Base.Systems;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace Base.Scenes.Preload
{
    public class SplashController : Controller<SplashScreen, SplashModel, SplashView>
    {
        private CancellationTokenSource _cancellationTokenSource;

        public SplashController(SplashScreen splashScreen, SplashModel model, SplashView view)
            : base(splashScreen, model, view)
        {
        }

        public override async void Initialize()
        {
            base.Initialize();
            _cancellationTokenSource = new CancellationTokenSource();
            await SimulateSplashAnimation();
            CloseScreen();
        }

        public override void Dispose()
        {
            base.Dispose();
            _cancellationTokenSource.Dispose();
        }

        private async UniTask SimulateSplashAnimation()
        {
            await UniTask.Delay(_model.AnimationSimulationTimerMilliseconds, cancellationToken: _cancellationTokenSource.Token);
        }
    }
}