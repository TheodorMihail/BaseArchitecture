using Cysharp.Threading.Tasks;
using System;
using Zenject;

namespace Base.Systems
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
        void LogError(string message, Exception ex = null);
    }
}
