using Base.Systems;

namespace Base.Scenes.GamePreload
{
    public class BootController : Controller<BootScreen, BootModel, BootView>
    {
        public BootController(BootScreen bootScreen, BootModel model, BootView view)
            : base(bootScreen, model, view)
        {
        }

        public override async void Initialize()
        {
            base.Initialize();
            //Simulating a server data, addressables or any other kind of initialization loading 
            await _view.PlayLoadingAnimation(_model.DelayTimerSeconds);
            CloseScreen();
        }
    }
}