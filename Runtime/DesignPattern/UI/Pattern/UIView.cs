using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Kit.UI
{
    public interface IView
    {
    }


    public interface IElement
    {
        int Id { get; set; }
        bool IsLoading { get; }
    }

    /// <summary>
    /// 화면에 보여지는 모든 렌더러들을 여기서 관리한다
    /// </summary>
    public abstract class UIView : UIContainer
    {
        public IElement FindElementByIdInChildren(int id, bool includeInactive = false)
        {
            return this.transform.GetComponentsInChildren<IElement>(includeInactive).First(x => x.Id == id);
        }

        public T FindElementByIdInChildren<T>(int id, bool includeInactive = false) where T : IElement
        {
            return this.transform.GetComponentsInChildren<T>(includeInactive).First(x => x.Id == id);
        }
    }
}