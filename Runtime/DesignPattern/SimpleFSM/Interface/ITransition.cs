using Cysharp.Threading.Tasks;

namespace Kit
{
    public interface ITransition
    { 
        /// <summary>
        /// 트랜지션 가능여부
        /// </summary>
        /// <returns></returns>
        UniTask<bool> ShouldTransition();
    }
}