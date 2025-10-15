using Cysharp.Threading.Tasks;
using System;
using System.Runtime.CompilerServices;
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
        /// <typeparam name="T">The type of the calling class for context</typeparam>
        /// <param name="message">Error message</param>
        /// <param name="ex">Optional exception</param>
        /// <param name="memberName">Automatically captured calling method name</param>
        void LogError<T>(string message, Exception ex = null, [CallerMemberName] string memberName = "");
    }
}
