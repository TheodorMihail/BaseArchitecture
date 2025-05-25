using Base.Project;
using Base.Systems;
using static Base.Scenes.MainMenu.CharacterSelectionScreen;

namespace Base.Scenes.MainMenu
{
    public class CharacterSelectionController : Controller<CharacterSelectionScreen, CharacterSelectionModel, CharacterSelectionView, CharacterSelectionScreenResult>
    {
        private IAuthenticationManager _authenticationManager;

        public CharacterSelectionController(CharacterSelectionScreen CharacterSelectionScreen, CharacterSelectionModel model, CharacterSelectionView view,
            IAuthenticationManager authenticationManager)
            : base(CharacterSelectionScreen, model, view)
        {
            _authenticationManager = authenticationManager;
        }

        public override void Initialize()
        {
            base.Initialize();
            _view.OnCharacterSelectedBtnPressed += OnCharacterSelectedBtnPressed;
            _view.OnCharacterSelectionCanceledBtnPressed += OnCharacterSelectionCanceledBtnPressed;
        }

        public override void Dispose()
        {
            _view.OnCharacterSelectedBtnPressed -= OnCharacterSelectedBtnPressed;
            _view.OnCharacterSelectionCanceledBtnPressed -= OnCharacterSelectionCanceledBtnPressed;
            base.Dispose();
        }

        private void OnCharacterSelectedBtnPressed(string characterUID)
        {
            CloseScreen(new CharacterSelectionScreenResult() 
            { 
                State = ResultType.CharacterSelectionCanceled, 
                CharacterUID = characterUID
            });
        }

        private void OnCharacterSelectionCanceledBtnPressed()
        {
            _authenticationManager.LogOut();
            CloseScreen(new CharacterSelectionScreenResult() { State = ResultType.CharacterSelectionCanceled });
        }
    }
}