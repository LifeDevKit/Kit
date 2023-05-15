
using System.Collections.Generic; 
using UnityEngine.UIElements;
using UnityEngine;

namespace Kit.Editor
{
    [UnityEditor.CustomEditor(typeof(UnityEngine.Canvas))]
    internal class CanvasScalerEditor : UnityEditor.Editor
    { 
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        } 

        public override VisualElement CreateInspectorGUI()
        {
            var baseGUI = base.CreateInspectorGUI();
 
            
            if (baseGUI == null)
            {
                baseGUI = new VisualElement();
                baseGUI.Add(new IMGUIContainer(OnInspectorGUI));
            }


            var element = new VisualElement();
            element.style.display = new StyleEnum<DisplayStyle>()
            {
                value = DisplayStyle.Flex
            };
            element.style.flexDirection = FlexDirection.Column;
            element.style.backgroundColor = new Color(.1f, .1f, .1f);
            element.style.color = Color.white;
            
            
            element.Add(new Button(() => { })
            {
                text = "KIT Override Sample"
            });
            baseGUI.Add(element);
            return baseGUI;
        }
    }
}
