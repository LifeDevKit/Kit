 
using UnityEngine;

namespace Kit.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class UISafeAreaTransform : MonoBehaviour
    { 
        RectTransform m_rectTransform;

        private void Awake()
        {
            m_rectTransform = GetComponent<RectTransform>(); 
           
        }

        
        private void OnValidate()
        { 
             Debug.Log("OnValidate");
        }

        private void OnEnable()
        {
            Debug.Log("OnEnable");
        }

        private void OnDisable()
        {
            Debug.Log("OnDisable");
        }

        public void Resize()
        {
            
        }
    }
}