using System;

namespace BaseArchitecture.Core
{
    public interface IState<T> where T : Enum
    {
        T Id { get; }
        event Action<(T stateId, object[] paramsList)> OnStateFinished;
        void OnEnter(params object[] paramsList);
        void OnUpdate();
        void OnExit();
    }

    /// <summary>
    /// Base class for state machine states. A state runs until it calls FinishState, which is what
    /// triggers the transition.
    /// </summary>
    public abstract class BaseState<T> : IState<T>
        where T : Enum
    {
        public abstract T Id { get; }
        public event Action<(T stateId, object[] paramsList)> OnStateFinished;

        public virtual void OnEnter(params object[] paramsList) { }
        public virtual void OnUpdate() { }
        public virtual void OnExit() { }

        /// <summary>
        /// Marks this state complete. The parameters reach the state machine's OnStateFinished.
        /// </summary>
        protected void FinishState(params object[] paramsList)
        {
            OnStateFinished.Invoke((Id, paramsList));
        }
    }
}