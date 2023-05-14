using System.Collections.Generic;
using Kit.UI.Pattern;
using UnityEngine;

namespace Kit.UI
{
    public interface IPopup
    {
        public Kit.IManager PopupManager { get; }
        void Show();
        void Hide();
    }
 
    
    public class UIPopup<TEvent> : UIContainer, IPopup 
        where TEvent : struct
    {
        public bool isShow = false;
        public IManager PopupManager { get; }

        public void Show()
        {
            isShow = true;
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