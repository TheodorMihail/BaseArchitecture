using Base.Project;
using Base.Systems;
using UnityEngine;
using Zenject;

namespace Base.Scenes.Preload
{
    public class PreloadInstaller : MonoInstaller
    {
        public Transform ScreensContainer;

        public override void InstallBindings()
        {
            ContainersInstall();
            StateMachineInstall();
        }

        private void ContainersInstall()
        {
            Container.Bind<Transform>().WithId(IScreen.ScreensContainerID)
                .FromInstance(ScreensContainer).AsSingle();

            Container.Resolve<IUIManager>().UpdateDIContainer(Container);
        }

        private void StateMachineInstall()
        {
            Container.BindInterfacesTo<BootState>().AsSingle();
            Container.BindInterfacesTo<SplashState>().AsSingle();
            Container.BindInterfacesTo<PreloadStateMachine>().AsSingle();
        }
    }
}