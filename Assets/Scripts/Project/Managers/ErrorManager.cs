using Base.Systems;
using Cysharp.Threading.Tasks;
using System;
using System.Runtime.CompilerServices;
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
            var errorParams = new ErrorDialogScreenParams { Message = message };
            await _uiManager.ShowScreen<ErrorDialogScreen, ErrorDialogScreenParams>(errorParams);
        }

        public void LogError<T>(
            string message,
            Exception ex = null,
            [CallerMemberName] string memberName = "")
        {
            var typeName = typeof(T).Name;
            var context = $"{typeName}.{memberName}";

            if (ex != null)
            {
                Debug.LogError($"[{context}] [Error] {message}\n{ex}");
            }
            else
            {
                Debug.LogError($"[{context}] [Error] {message}");
            }
        }
    }
}
