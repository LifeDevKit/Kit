using System;
using UnityEngine;
namespace Kit.Core
{
    [RequireComponent(typeof(KitEntity))]
    public abstract class EntityBaseController : MonoBehaviour, IKitController
    {
        public KitEntity entity;

        public void Awake()
        {
            entity ??= GetComponent<KitEntity>();
        }

        void Update()
        {
            this.UpdateController();
        }

        public void UpdateController()
        {
            throw new NotImplementedException();
        }
    }
}