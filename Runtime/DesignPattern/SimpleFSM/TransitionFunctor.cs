using System;
using Cysharp.Threading.Tasks;

namespace Kit
{
    /// <summary>
    /// 콜백 기반으로 동작하는 트렌지션
    /// </summary>
    public class TransitionFunctor : ITransition
    {
        public Func<UniTask<bool>> Functor;

        public TransitionFunctor(Func<UniTask<bool>> functor)
        {
            Functor = functor;
        }

        public async UniTask<bool> ShouldTransition() => await Functor();
        
    }
}