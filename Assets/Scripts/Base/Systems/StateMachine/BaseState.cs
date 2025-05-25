using System;

namespace Base.Systems
{
    public interface IState<T> where T : Enum
    {
        T Id { get; }
        event Action<(T stateId, object[] paramsList)> OnStateFinished;
        void OnEnter();
        void OnUpdate();
        void OnExit();
    }

    public abstract class BaseState<T> : IState<T>
        where T : Enum
    {
        public abstract T Id { get; }
        public event Action<(T stateId, object[] paramsList)> OnStateFinished;

        public virtual void OnEnter() { }
        public virtual void OnUpdate() { }
        public virtual void OnExit() { }

        protected void FinishState(params object[] paramsList)
        {
            OnStateFinished.Invoke((Id, paramsList));
        }
    }
}