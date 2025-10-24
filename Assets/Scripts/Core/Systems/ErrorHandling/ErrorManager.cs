using BaseArchitecture.Core.Screens;
using Cysharp.Threading.Tasks;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using Zenject;
using static BaseArchitecture.Core.Screens.ErrorDialogScreen;

namespace BaseArchitecture.Core
{
    /// <summary>
    /// Manages error handling, logging, and user-facing error dialogs.
    /// Provides consistent error reporting across the application.
    /// </summary>
    public interface IErrorManager : IInitializable, IDisposable
    {
        /// <summary>
        /// Shows an error dialog and logs the error. Returns when user acknowledges.
        /// </summary>
        UniTask ShowErrorDialog(string message);

        /// <summary>
        /// Logs an error without showing dialog. Use for non-critical errors.
        /// </summary>
        void LogError<T>(string message, Exception ex = null, bool includeMethod = false, [CallerMemberName] string memberName = "");
    }
    
    public class ErrorManager : IErrorManager
    {
        [Inject] private readonly IUIManager _uiManager;

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
            bool includeMethod = false,
            [CallerMemberName] string memberName = "")
        {
            var typeName = typeof(T).Name;
            var context = includeMethod ? $"{typeName}.{memberName}" : typeName;

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
