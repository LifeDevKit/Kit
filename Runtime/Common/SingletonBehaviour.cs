using UnityEngine;


namespace Kit
{
    public abstract class SingletonBehaviour<T> : MonoBehaviour where T : Component
    {
        public virtual string DefaultObjectName
        {
            get
            {
                return $"{this.GetType().Name}(SingletonBehaviour)";
            }
        }

        protected virtual bool IsDontDestroy
        {
            get
            {
                return true;
            }
        }
         
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameObject().AddComponent<T>();  
                    return _instance;
                }

                return _instance;
            }
        } 
        private static T _instance;
 
        public virtual void Awake()
        {
            var singleton = _instance as SingletonBehaviour<T>;
            _instance.gameObject.name = singleton.DefaultObjectName; 
            if (IsDontDestroy)
            {
                DontDestroyOnLoad(this.gameObject);
            }
        } 
    }
}