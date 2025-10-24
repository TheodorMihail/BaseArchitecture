using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BaseArchitecture.Core.Screens
{
    [AddressablePath("Screens/ErrorDialogScreenView")]
    public class ErrorDialogView : View
    {
        [SerializeField] private TMP_Text _messageText;
        [SerializeField] private Button _confirmButton;

        public event Action OnConfirmPressed;

        private void Awake()
        {
            _confirmButton.onClick.AddListener(() => OnConfirmPressed?.Invoke());
        }

        public void SetMessage(string message)
        {
            _messageText.text = message;
        }
    }
}
