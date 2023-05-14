using Kit.Events;

namespace Kit
{
    public interface IStateMachine<TStateKey> : IKitUnityUpdateEvent
        where TStateKey : struct
    {
        void Initialize(TStateKey defaultState);
        void Add(TStateKey key, IState state);
        void Remove(TStateKey key);

        void AddTransition(TStateKey from, TStateKey to, ITransition transition);
        void AddTransition(TStateKey from, TStateKey to, System.Func<bool> shouldTransitionPredicate);
        
        
    }
}