using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace BaseArchitecture.Core
{
    public interface IStateMachine<T> : IInitializable, IDisposable, ITickable
        where T : Enum
    {
    }

    /// <summary>
    /// Base class for application and scene state flows. Handles the transitions, the state
    /// lifecycle and the event subscriptions; the subclass supplies the flow itself.
    /// </summary>
    public abstract class BaseStateMachine<T> : IStateMachine<T>
        where T : Enum
    {
        protected abstract T DefaultStateId { get; }

        /// <summary>
        /// Called when a state completes, with whatever parameters it finished with. This is where the
        /// next SetState is decided.
        /// </summary>
        protected abstract void OnStateFinished((T stateId, object[] paramsList) finishedState);

        protected readonly IList<IState<T>> _states;
        protected IState<T> _activeState;

        public BaseStateMachine(IList<IState<T>> statesList)
        {
            _states = statesList;
        }

        public virtual void Initialize()
        {
            SetState(DefaultStateId);
        }

        public virtual void Dispose()
        {
            _activeState?.OnExit();
        }

        public virtual void Tick()
        {
            if (_activeState != null)
                UpdateActiveState();
        }

        protected void UpdateActiveState()
        {
            _activeState?.OnUpdate();
        }

        protected void SetState(T stateId, params object[] paramsList)
        {
            var state = _states.FirstOrDefault(s => s.Id.Equals(stateId));
            if (state == null)
                throw new Exception($"Could not set state {stateId}");

            this.Log($"Transition to state: {stateId}");
            TransitionToNextState(state, paramsList);
        }
        
        protected void TransitionToNextState(IState<T> nextState, params object[] paramsList)
        {
            DisposeCurrentState();

            _activeState = nextState;
            _activeState.OnStateFinished += OnStateFinished;
            _activeState.OnEnter(paramsList);
        }

        protected void DisposeCurrentState()
        {
            if (_activeState == null)
                return;

            _activeState.OnStateFinished -= OnStateFinished;
            _activeState.OnExit();
            _activeState = default;
        }
    }
}