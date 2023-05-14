using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kit
{
    public abstract class State
    { 
        public abstract void OnEnter();
        public abstract void OnUpdate();
        public abstract void OnExit(); 
    }
}
