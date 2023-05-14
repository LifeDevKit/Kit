using System;

namespace Kit
{
    public class TransitionFunctor : ITransition
    {
        public Func<bool> Functor;

        public TransitionFunctor(Func<bool> functor)
        {
            Functor = functor;
        }

        public bool ShouldTransition() => Functor.Invoke();
    }
}