using Base.Systems;
using static Base.Scenes.MainMenu.CharacterSelectionScreen;

namespace Base.Scenes.MainMenu
{
    public class CharacterSelectionScreen : Screen<CharacterSelectionModel, CharacterSelectionView, CharacterSelectionController, CharacterSelectionScreenResult>
    {
        public enum ResultType
        {
            CharacterSelectionCanceled,
            CharacterSelected
        }

        public struct CharacterSelectionScreenResult : IScreenResult
        {
            public ResultType State;
            public string CharacterUID;
        }
    }
}