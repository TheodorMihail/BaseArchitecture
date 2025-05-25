using Base.Systems;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Base.Scenes.MainMenu
{
    [AddressablePath("Screens/MenuView")]
    public class MenuView : View
    {
        [SerializeField] private Button _backBtn;

        public event Action<string> OnMapSelectedBtnPressed;
        public event Action OnMapSelectionCanceledBtnPressed;

        private void Awake()
        {
            _backBtn.onClick.AddListener(() => OnMapSelectionCanceledBtnPressed?.Invoke());
        }
    }
}