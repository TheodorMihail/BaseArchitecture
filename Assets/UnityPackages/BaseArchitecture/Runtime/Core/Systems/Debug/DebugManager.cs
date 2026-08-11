#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System.Collections.Generic;
using System.Text;
using UnityEngine.InputSystem;
using Zenject;

namespace BaseArchitecture.Core
{
    /// <summary>
    /// Owns the debug command table for the scene it is bound in and dispatches key presses to the
    /// provider that declared them. Bind it per scene rather than in the project context, so each
    /// instance collects the project-scope providers plus whatever that scene contributes.
    /// Compiled out of release builds.
    /// </summary>
    public class DebugManager : IInitializable, ITickable
    {
        [Inject] private readonly List<IDebugCommandProvider> _providers;

        private readonly List<DebugCommandDTO> _commands = new List<DebugCommandDTO>();

        public void Initialize()
        {
            foreach (IDebugCommandProvider provider in _providers)
            {
                foreach (DebugCommandDTO command in provider.GetDebugCommands())
                {
                    if (_commands.Exists(existing => existing.Key == command.Key))
                    {
                        this.LogError($"Debug key {command.Key} is already bound. '{command.Label}' ignored.");
                        continue;
                    }

                    _commands.Add(command);
                }
            }

            LogCommands();
        }

        /// <summary>
        /// Lists everything bound in this scene, so the active keymap never has to be looked up in code.
        /// </summary>
        private void LogCommands()
        {
            var sorted = new List<DebugCommandDTO>(_commands);
            sorted.Sort((first, second) => first.Key.CompareTo(second.Key));

            var builder = new StringBuilder($"Debug commands ({sorted.Count}):");
            foreach (DebugCommandDTO command in sorted)
            {
                builder.AppendLine();
                builder.Append($"  {command.Key} - {command.Label}");
            }

            this.Log(builder.ToString());
        }

        public void Tick()
        {
            if (Keyboard.current == null)
            {
                return;
            }

            foreach (DebugCommandDTO command in _commands)
            {
                if (Keyboard.current[command.Key].wasPressedThisFrame)
                {
                    this.LogWarning($"Debug: {command.Label} ({command.Key})");
                    command.Action();
                }
            }
        }
    }
}
#endif
