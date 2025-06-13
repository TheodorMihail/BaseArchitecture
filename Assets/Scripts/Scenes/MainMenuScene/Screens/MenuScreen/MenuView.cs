using Base.Systems;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Base.Scenes.MainMenu
{
    [AddressablePath("Screens/MenuView")]
    public class MenuView : View
    {
        [SerializeField] private Button _backBtn;
        [SerializeField] private MapSlot _mapSlotPrefab;
        [SerializeField] private Transform _mapSlotsContainer;

        public event Action<string> OnMapSelectedBtnPressed;
        public event Action OnMapSelectionCanceledBtnPressed;

        private void Awake()
        {
            _backBtn.onClick.AddListener(() => OnMapSelectionCanceledBtnPressed?.Invoke());
        }

        public void SetupMaps(List<string> mapIds)
        {
            foreach(var id in mapIds)
            {
                var map = Instantiate(_mapSlotPrefab, _mapSlotsContainer);
                map.Initialize(id);
                map.OnMapSelectedBtnPressed += OnMapSelectedBtnPressed;
            }
        }
    }
}