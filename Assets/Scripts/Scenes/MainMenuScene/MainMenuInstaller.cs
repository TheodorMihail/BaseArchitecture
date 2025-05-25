using Base.Project;
using Base.Systems;
using UnityEngine;
using Zenject;

namespace Base.Scenes.MainMenu
{
    public class MainMenuInstaller : MonoInstaller
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
            Container.BindInterfacesTo<AuthenticationState>().AsSingle();
            Container.BindInterfacesTo<CharactersState>().AsSingle();
            Container.BindInterfacesTo<MainMenuStateMachine>().AsSingle();
        }
    }
}