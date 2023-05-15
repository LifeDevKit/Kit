using System;
using System.Collections.Generic;
using ImGuiNET;
using Kit.Utility;
using UImGui;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Kit.UI
{ 
    /// <summary>
    /// UI 전용 컨테이너 클래스.
    /// 핵심이되는 모든 UI는 이 컨테이너를 사용합니다.
    /// </summary>
    [RequireComponent(typeof(RectTransform))] 
    public abstract class UIContainer : UIBehaviour, ICanvasElement
    {  
        [SerializeField] RectTransform _rectTransform;
        public RectTransform RectTransform => _rectTransform;
        protected virtual void OnValidate()
        {
            InitializeDefaultComponents();
        }
 
        protected override void OnEnable()
        {
#if UNITY_EDITOR
            UImGuiUtility.Layout += OnImGUILayout;
#endif
        } 
        protected override void OnDisable()
        {
#if UNITY_EDITOR
            UImGuiUtility.Layout -= OnImGUILayout;
#endif
        }

        protected override void OnCanvasHierarchyChanged()
        {
            base.OnCanvasHierarchyChanged();
            this.Log("OnCanvasHierarchyChanged");
        }

        protected override void OnRectTransformDimensionsChange()
        { 
            base.OnRectTransformDimensionsChange();
            this.Log("OnRectTransformDimensionsChange");
        }

        /// <summary>
        /// 캔버스 그룹 변경시 
        /// </summary>
        protected override void OnCanvasGroupChanged()
        { 
            base.OnCanvasGroupChanged();
            this.Log("OnCanvasGroupChanged");
        }

        /// <summary>
        /// 카메라에 표시되기전에 호출해야하는 콜백
        /// </summary> 
        private void OnBecameVisible()
        {
             this.Log("OnBecameVisible");
        }

        public virtual void OnImGUILayout(UImGui.UImGui context)
        {
#if UNITY_EDITOR
            if (!UnityEditor.Selection.activeGameObject) return;
            
                ImGui.Begin($"{this.GetType().Name}");
  
                ImGui.EndMenu(); 
                 
                ImGui.ShowDemoWindow();
                ImGui.End(); 
#endif
        }

 
        private void InitializeDefaultComponents()
        { 
            _rectTransform ??= GetComponent<RectTransform>();
        }
         
        public virtual void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.matrix = this._rectTransform.localToWorldMatrix; 
            Gizmos.DrawLine(_rectTransform.rect.min, _rectTransform.rect.max);
        }

        public virtual void AddToChildren(RectTransform other)
        {
            other.transform.SetParent(this.transform, false);
        }


        public virtual void Rebuild(CanvasUpdate executing)
        {
            Debug.Log(executing);
        }

        public virtual void LayoutComplete()
        {
            Debug.Log("LayoutComplete");
        }

        public virtual void GraphicUpdateComplete()
        {
           Debug.Log("GraphicUpdateComplete");
        }
    }
 
}