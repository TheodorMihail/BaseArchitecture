using Base.Systems;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using static Base.Project.ErrorDialogScreen;

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
            LogError(message);

            // Show dialog and wait for user to close it
            var errorParams = new ErrorDialogScreenParams { Message = message };
            await _uiManager.ShowScreen<ErrorDialogScreen, ErrorDialogScreenParams>(errorParams);
        }

        public void LogError(string message, Exception ex = null)
        {
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
