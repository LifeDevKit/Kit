using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Kit
{
    public interface IState
    { 
        public IStateMachine StateMachine { get; set; }
        UniTask OnStateEnter();
        void OnStateUpdate();
        UniTask OnStateExit(); 
    }
 
    
}