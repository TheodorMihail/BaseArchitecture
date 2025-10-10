using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Base.Systems
{
    public interface IStateMachine<T> : IInitializable, IDisposable, ITickable
        where T : Enum
    {
    }

    /// <summary>
    /// Generic state machine base class for managing application and scene state flows.
    /// Automatically handles state transitions, lifecycle management, and event subscriptions.
    /// Implement DefaultStateId and OnStateFinished to define your state flow logic.
    /// </summary>
    public abstract class BaseStateMachine<T> : IStateMachine<T>
        where T : Enum
    {
        protected abstract T DefaultStateId { get; }

        /// <summary>
        /// Called when a state completes. Implement this to define state transition logic.
        /// Use SetState() to transition to the next state based on the finished state and its parameters.
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

        protected void TransitionToNextState(IState<T> nextState)
        {
            DisposeCurrentState();

            _activeState = nextState;
            _activeState.OnStateFinished += OnStateFinished;
            _activeState.OnEnter();
        }

        protected void DisposeCurrentState()
        {
            if (_activeState == null)
                return;

            _activeState.OnStateFinished -= OnStateFinished;
            _activeState.OnExit();
            _activeState = default;
        }

        protected void SetState(T stateId)
        {
            var state = _states.FirstOrDefault(s => s.Id.Equals(stateId));
            if (state == null)
                throw new Exception($"Could not set state {stateId}");

            Debug.Log($"Transition to state: {stateId}");
            TransitionToNextState(state);
        }
    }
}