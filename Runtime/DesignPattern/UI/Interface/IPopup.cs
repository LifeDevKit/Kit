namespace Kit.UI
{
    public interface IPopup
    {
        void UpdatePopup<T>(T data);
        void Show();
        void Hide();
    } 
}