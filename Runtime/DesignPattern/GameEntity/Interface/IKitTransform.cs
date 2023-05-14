using UnityEngine;

namespace Kit.Core
{

    public interface IKitTransform
    {
        Vector3 Position { get; set; }
        Transform Root { get; }
    }

}


