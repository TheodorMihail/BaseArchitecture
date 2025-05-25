using Base.Systems;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Base.Scenes.MainMenu
{
    [AddressablePath("Screens/CharacterSelectionView")]
    public class CharacterSelectionView : View
    {
        [SerializeField] private Button _confirmBtn;
        [SerializeField] private Button _cancelBtn;

        public event Action<string> OnCharacterSelectedBtnPressed;
        public event Action OnCharacterSelectionCanceledBtnPressed;

        private void Awake()
        {
            _confirmBtn.onClick.AddListener(() => OnCharacterSelectedBtnPressed?.Invoke(""));
            _cancelBtn.onClick.AddListener(() => OnCharacterSelectionCanceledBtnPressed?.Invoke());
        }
    }
}