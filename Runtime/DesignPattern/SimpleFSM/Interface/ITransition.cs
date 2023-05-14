namespace Kit
{
    public interface ITransition
    { 
        /// <summary>
        /// 트랜지션 가능여부
        /// </summary>
        /// <returns></returns>
        bool ShouldTransition();
    }
}