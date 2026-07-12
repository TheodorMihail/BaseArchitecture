using System;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using UnityEngine;

namespace BaseArchitecture.Core
{
    /// <summary>
    /// Extension methods for logging that include type and optional method context.
    /// </summary>
    public static class LoggingExtensions
    {
        /// <summary>
        /// Logs a standard message with type context.
        /// </summary>
        [HideInCallstack]
        public static void Log<T>(this T caller, string message, bool includeMethod = false, [CallerMemberName] string memberName = "")
        {
            var typeName = caller.GetType().Name;
            var context = includeMethod ? $"{typeName}.{memberName}" : typeName;
            Debug.Log($"[{context}] {message}");
        }

        /// <summary>
        /// Logs an object serialized to JSON for debugging.
        /// </summary>
        [HideInCallstack]
        public static void LogObject<T>(this T caller, object obj, bool includeMethod = false, [CallerMemberName] string memberName = "")
        {
            var json = JsonConvert.SerializeObject(obj, Formatting.Indented);
            Log(caller, json, includeMethod, memberName);
        }

        /// <summary>
        /// Logs a warning message with type context.
        /// </summary>
        [HideInCallstack]
        public static void LogWarning<T>(this T caller, string message, bool includeMethod = false, [CallerMemberName] string memberName = "")
        {
            var typeName = caller.GetType().Name;
            var context = includeMethod ? $"{typeName}.{memberName}" : typeName;
            Debug.LogWarning($"[{context}] [Warning] {message}");
        }

        /// <summary>
        /// Logs an error message with type context.
        /// </summary>
        [HideInCallstack]
        public static void LogError<T>(this T caller, string message, Exception ex = null, bool includeMethod = false, [CallerMemberName] string memberName = "")
        {
            var typeName = caller.GetType().Name;
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
