using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Kit
{
    public interface IState
    {

        void OnStateEnter();
        UniTask OnStateUpdate();
        void OnStateExit();
    }
}