using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Kit.UI;
using UnityEngine;

namespace Kit.UI
{
    public abstract class UIPopup : UIContainer, IPopup 
    {
        public bool isShow = false;
        public IKitManager PopupKitManager { get; }

        public virtual void UpdatePopup<T>(T data)
        {
            throw new NotImplementedException("팝업 업데이트가 별도로 구현되지 않음.");
        }
  
        public void Show()
        { 
        }

        public void Hide()
        {
            isShow = false;
        }
 
        
         

        /// <summary>
        /// 팝업이 보여지면서 실행될 이벤트 ex) 트윈
        /// </summary>
        protected virtual void OnShow()
        {
            
        }

        
        /// <summary>
        /// 팝업이 가려지면서 실행될 이벤트
        /// </summary>
        protected virtual void OnHide()
        {
            
        }
        
        
    }

    public class UIPopupManager : MonoBehaviour
    {
        public Stack<IPopup> currentPopups = new Stack<IPopup>();

        public void Show(IPopup popup)
        {
            popup.Show();
            currentPopups.Push(popup);
        }
        public void Hide(IPopup popup)
        {
            popup.Hide();
            var popped = currentPopups.Pop(); 
        }
    }
}