namespace Kit.Services.Auth
{
    public interface IContext
    {
        
    }
    public interface IAuthProvider
    {
        public IContext context;
    }
}