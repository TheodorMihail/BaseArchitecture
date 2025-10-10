using Base.Project;
using Base.Systems;
using UnityEngine;
using Zenject;

namespace Base.Scenes.GamePreload
{
    /// <summary>
    /// Scene-level Zenject installer. Binds screen containers and state machine components.
    /// Must call UpdateDIContainer() to enable UIManager screen instantiation in scene context.
    /// </summary>
    public class GamePreloadInstaller : MonoInstaller
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
            Container.BindInterfacesTo<GamePreloadStateMachine>().AsSingle();
        }
    }
}