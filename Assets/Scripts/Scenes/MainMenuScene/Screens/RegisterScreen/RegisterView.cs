using Base.Systems;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Base.Scenes.MainMenu
{
    [AddressablePath("Screens/RegisterView")]
    public class RegisterView : View
    {
        [SerializeField] private GameObject _registerPanel;
        [SerializeField] private TMP_InputField _usernameInputField;
        [SerializeField] private TMP_InputField _passwordInputField;
        [SerializeField] private Button _confirmBtn;
        [SerializeField] private Button _cancelBtn;

        [SerializeField] private GameObject _loadingPanel;

        public event Action<(string username, string password)> OnRegisterConfirmBtnPressed;
        public event Action OnRegisterCancelBtnPressed;

        private const int _minCharactersLength = 3;

        private string Username => _usernameInputField.text;
        private string Password => _passwordInputField.text;

        private void Awake()
        {
            _usernameInputField.onValueChanged.AddListener(OnInputFieldValueChanged);
            _passwordInputField.onValueChanged.AddListener(OnInputFieldValueChanged);
            _confirmBtn.onClick.AddListener(() => OnRegisterConfirmBtnPressed?.Invoke((Username, Password)));
            _cancelBtn.onClick.AddListener(() => OnRegisterCancelBtnPressed?.Invoke());
        }

        public void ShowRegisterPanel()
        {
            ClearInputFields();
            _registerPanel.SetActive(true);
            _loadingPanel.SetActive(false);
        }

        public void ShowLoadingPanel()
        {
            _registerPanel.SetActive(false);
            _loadingPanel.SetActive(true);
        }

        private void OnInputFieldValueChanged(string text)
        {
            bool enableConfirmBtn = Username.Length >= _minCharactersLength 
                && Password.Length >= _minCharactersLength;

            _confirmBtn.interactable = enableConfirmBtn;
        }

        private void ClearInputFields()
        {
            _confirmBtn.interactable = false;
            _usernameInputField.text = "";
            _passwordInputField.text = "";
        }
    }
}