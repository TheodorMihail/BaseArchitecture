using System;
using System.Collections.Generic;
using Zenject;

namespace BaseArchitecture.Core
{
    /// <summary>
    /// Message bus interface for publish/subscribe messaging pattern.
    /// Allows decoupled communication between systems using typed messages.
    /// </summary>
    public interface IMessageBus : IInitializable, IDisposable
    {
        void Subscribe<TMessage>(Action<TMessage> handler) where TMessage : IMessageObject;
        void Unsubscribe<TMessage>(Action<TMessage> handler) where TMessage : IMessageObject;
        void Publish<TMessage>(TMessage message) where TMessage : IMessageObject;
    }

    /// <summary>
    /// Central message bus implementation for publish/subscribe pattern.
    /// Enables decoupled communication between systems using typed messages.
    /// </summary>
    public class MessageBus : IMessageBus
    {
        private readonly Dictionary<Type, List<object>> _subscribers = new Dictionary<Type, List<object>>();

        public void Initialize()
        {
        }

        public void Dispose()
        {
            _subscribers.Clear();
        }

        public void Subscribe<TMessage>(Action<TMessage> handler) where TMessage : IMessageObject
        {
            var messageType = typeof(TMessage);

            if (!_subscribers.ContainsKey(messageType))
                _subscribers[messageType] = new List<object>();

            if (_subscribers[messageType].Contains(handler))
            {
                this.LogWarning($"Handler already subscribed for {typeof(TMessage).Name}. Ignoring duplicate.");
                return;
            }

            _subscribers[messageType].Add(handler);
        }

        public void Unsubscribe<TMessage>(Action<TMessage> handler) where TMessage : IMessageObject
        {
            var messageType = typeof(TMessage);

            if (_subscribers.ContainsKey(messageType))
            {
                _subscribers[messageType].Remove(handler);

                if (_subscribers[messageType].Count == 0)
                {
                    _subscribers.Remove(messageType);
                }
            }
        }

        public void Publish<TMessage>(TMessage message) where TMessage : IMessageObject
        {
            var messageType = typeof(TMessage);

            if (!_subscribers.ContainsKey(messageType))
                return;

            // Snapshot before dispatch so Subscribe/Unsubscribe calls from within
            // handlers don't corrupt iteration or cause double-invocations.
            var snapshot = new List<object>(_subscribers[messageType]);

            foreach (var handler in snapshot)
            {
                try
                {
                    ((Action<TMessage>)handler).Invoke(message);
                }
                catch (Exception ex)
                {
                    UnityEngine.Debug.LogError($"[MessageBus] Error invoking handler for {typeof(TMessage).Name}: {ex}");
                }
            }
        }
    }
}
