using Base.Systems;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace Base.Project
{
    public class ErrorManager : IErrorManager
    {
        private readonly IUIManager _uiManager;

        public ErrorManager(IUIManager uiManager)
        {
            _uiManager = uiManager;
        }

        public void Initialize()
        {
        }

        public void Dispose()
        {
        }

        public async UniTask ShowErrorDialog(string message)
        {
            Debug.LogError($"[Error] {message}");

            // Show dialog and wait for user to close it
            await _uiManager.ShowScreen<ErrorDialogScreen>(message);
        }

        public void LogError(string message, Exception ex = null)
        {
            _uiManager.ShowScreen<ErrorDialogScreen>(message);
            if (ex != null)
            {
                Debug.LogError($"[Error] {message}\n{ex}");
            }
            else
            {
                Debug.LogError($"[Error] {message}");
            }
        }
    }
}
