using Base.Project;
using Base.Systems;
using System.Collections.Generic;
using static Base.Scenes.MainMenu.MenuScreen;

namespace Base.Scenes.MainMenu
{
    public class MenuController : Controller<MenuScreen, MenuModel, MenuView, MenuScreenResult>
    {
        private IAuthenticationManager _authenticationManager;
        private List<string> _mapData = new List<string>() { "1", "2" };

        public MenuController(MenuScreen menuScreen, MenuModel model, MenuView view,
            IAuthenticationManager authenticationManager)
            : base(menuScreen, model, view)
        {
            _authenticationManager = authenticationManager;
        }

        public override void Initialize()
        {
            base.Initialize();
            _view.OnMapSelectedBtnPressed += OnMapSelectedBtnPressed;
            _view.OnMapSelectionCanceledBtnPressed += OnMapSelectionCanceledBtnPressed;
            _view.SetupMaps(_mapData);
        }

        public override void Dispose()
        {
            _view.OnMapSelectedBtnPressed -= OnMapSelectedBtnPressed;
            _view.OnMapSelectionCanceledBtnPressed -= OnMapSelectionCanceledBtnPressed;
            base.Dispose();
        }

        private void OnMapSelectedBtnPressed(string mapUID)
        {
            CloseScreen(new MenuScreenResult() 
            { 
                State = ResultType.MapSelected, 
                MapUID = mapUID
            });
        }

        private void OnMapSelectionCanceledBtnPressed()
        {
            _authenticationManager.LogOut();
            CloseScreen(new MenuScreenResult() { State = ResultType.MapSelectionCanceled });
        }
    }
}