using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Kit
{
    public abstract class StateBase<TStateKey> : IState where TStateKey : struct
    {
        public GameObject _callerGameObject;
        public StateBase(GameObject caller)
        {
            this.CallerGameObject = caller;
        }
        public IStateMachine StateMachine { get; set; }

        public GameObject CallerGameObject
        {
            get => _callerGameObject;
            set => _callerGameObject = value;
        }

        public abstract UniTask OnStateEnter();
        public abstract void OnStateUpdate();

        public abstract UniTask OnStateExit();
    }
}