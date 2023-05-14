using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Kit.Core
{
    public abstract class KitEntityBase : MonoBehaviour, IKitTransform
    {
        public virtual Vector3 Position { get; set; }
        public virtual Transform Root
        {
            get
            {
                return this.transform;
            }
        }

    }
}