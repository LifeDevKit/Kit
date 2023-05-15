using Kit.UI;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Kit.UI
{
    public class UIVerticalLayout : UIContainer
    {
        [FormerlySerializedAs("content")] 
        [SerializeField]
        private RectTransform contentParent;

        public RectTransform ContentParent
        {
            get => contentParent;
            set => contentParent = value;
        }

        public VerticalLayoutGroup Group
        {
            get => LayoutGroup;
            set => LayoutGroup = value;
        }

        public VerticalLayoutGroup LayoutGroup;

        public override void AddToChildren(RectTransform other)
        {
            base.AddToChildren(other); 
        }
        
        
    }
}