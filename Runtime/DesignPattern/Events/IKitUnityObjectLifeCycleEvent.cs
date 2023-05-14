using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kit.Events
{
    public interface IKitUnityObjectLifeCycleEvent
    {
        void OnEnable();
        void OnDisable();
        void OnDestroy();
    }
}
