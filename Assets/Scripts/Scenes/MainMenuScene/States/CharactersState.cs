using Base.Project;
using Base.Systems;
using static Base.Scenes.MainMenu.CharacterSelectionScreen;
using static Base.Scenes.MainMenu.MainMenuStateMachine;

namespace Base.Scenes.MainMenu
{
    public class CharactersState : BaseState<MainMenuStateIds>
    {
        public override MainMenuStateIds Id => MainMenuStateIds.Characters;

        private readonly IUIManager _uiManager;

        public CharactersState(IUIManager uiManager)
        {
            _uiManager = uiManager;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            ShowCharacterSelectionScreen();
        }

        private async void ShowCharacterSelectionScreen()
        {
            var characterSelectionScreenResult = await _uiManager.ShowScreen<CharacterSelectionScreen, CharacterSelectionScreenResult>();
            if (characterSelectionScreenResult.State == ResultType.CharacterSelected)
            {
                FinishState(characterSelectionScreenResult.CharacterUID);
            }
            else
            {
                FinishState();
            }
        }
    }
}