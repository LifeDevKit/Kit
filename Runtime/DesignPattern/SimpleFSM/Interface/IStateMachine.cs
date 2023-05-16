﻿using Cysharp.Threading.Tasks;
using Kit.Events;

namespace Kit
{
    public interface IStateMachine
    {
        
    }

    public interface IStateMachine<TStateKey> : IStateMachine
        where TStateKey : struct
    { 
        void Add(TStateKey key, IState state);
        void Remove(TStateKey key);

        void AddTransition(TStateKey from, TStateKey to, ITransition transition);
        void AddTransition(TStateKey from, TStateKey to, System.Func<UniTask<bool>> shouldTransitionPredicate); 
    }
}