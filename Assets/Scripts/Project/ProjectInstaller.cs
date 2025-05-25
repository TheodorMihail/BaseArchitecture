using Base.Systems;
using Zenject;

namespace Base.Project
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            FactoriesInstall();
            ManagersInstall();
        }

        private void FactoriesInstall()
        {
            Container.BindInterfacesTo<Factory>().AsSingle().WithArguments(Container);
        }

        private void ManagersInstall()
        {
            Container.BindInterfacesTo<ScenesManager>().AsSingle();
            Container.BindInterfacesTo<UIManager>().AsSingle();
            Container.BindInterfacesTo<AddressablesManager>().AsSingle();
            Container.BindInterfacesTo<AuthenticationManager>().AsSingle();
        }
    }
}