namespace Kit
{
    /// <summary>
    /// 모든 매니저는 이 인터페이스를 통해서 구현할 수 있다.
    /// </summary>
    public interface IKitManager
    {
        bool Initialized { get; set; }
        void Initialize();
    }
}