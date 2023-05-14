using System.Collections.Generic;

namespace Kit
{
    public interface IState
    {

        void OnStateEnter();
        void OnStateUpdate();
        void OnStateExit();
    }
}