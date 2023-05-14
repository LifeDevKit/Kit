using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Kit.Pool
{ 
    public class ObjectPool : MonoBehaviour
    { 
        public Dictionary<GameObject, Stack<GameObject>> pools = new Dictionary<GameObject, Stack<GameObject>>();  
        public void RegisterToPool(GameObject prefab)
        {
            if (!pools.ContainsKey(prefab)) 
                pools.Add(prefab, new Stack<GameObject>()); 
        }
         
 
        public void Unregister(GameObject prefab, bool destroyCreatedObjects)
        {
            if (pools.ContainsKey(prefab))
            {
                if (destroyCreatedObjects)
                {
                    foreach (var created in pools[prefab])
                        GameObject.Destroy(created);
                }

                pools.Remove(prefab);
            }
        }

        public GameObject Get(GameObject prefab)
        {
            return default;
        }
        
        
    }
}