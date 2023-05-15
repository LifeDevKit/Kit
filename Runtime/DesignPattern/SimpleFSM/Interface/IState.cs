using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Kit
{
    public interface IState
    {
        IStateMachine StateMachine { get; set; }
        UniTask OnStateEnter();
        void OnStateUpdate();
        UniTask OnStateExit();
    }
}