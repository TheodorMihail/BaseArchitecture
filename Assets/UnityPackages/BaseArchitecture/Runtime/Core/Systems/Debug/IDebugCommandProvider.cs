#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;
using System.Collections.Generic;
using UnityEngine.InputSystem;

namespace BaseArchitecture.Core
{
    /// <summary>
    /// Implemented by anything exposing debug commands. Providers are collected by the debug
    /// manager bound in the active scene, which owns the dispatching.
    /// </summary>
    public interface IDebugCommandProvider
    {
        IReadOnlyList<DebugCommandDTO> GetDebugCommands();
    }

    /// <summary>
    /// A single debug command: the key that triggers it, a label for logging, and the action to run.
    /// </summary>
    public readonly struct DebugCommandDTO
    {
        public readonly Key Key;
        public readonly string Label;
        public readonly Action Action;

        public DebugCommandDTO(Key key, string label, Action action)
        {
            Key = key;
            Label = label;
            Action = action;
        }
    }
}
#endif
