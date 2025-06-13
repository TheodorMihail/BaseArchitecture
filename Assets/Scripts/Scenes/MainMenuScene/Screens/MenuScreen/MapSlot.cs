using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Base.Scenes.MainMenu
{
    public class MapSlot : MonoBehaviour
    {
        [SerializeField] private Button _selectBtn;
        [SerializeField] private TextMeshProUGUI _mapTitle;

        private string _currentId;
        public event Action<string> OnMapSelectedBtnPressed;

        private void Awake()
        {
            _selectBtn.onClick.AddListener(() => OnMapSelectedBtnPressed?.Invoke(_currentId));
        }

        public void Initialize(string id)
        {
            _currentId = id;
            _mapTitle.text = string.Format(_mapTitle.text, id);
        }
    }
}