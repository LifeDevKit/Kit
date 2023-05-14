namespace Kit.Services
{ 
    public class ServiceManager : SingletonBehaviour<ServiceManager>
    {
        public Kit.Services.Analytics.AnlyticsManager Analytics;
        public Kit.Services.Auth.AuthManager Auth;
        public Kit.Services.Notify.NotifyManager Notify;
        public Kit.Services.CDN.CDNManager CDN;

        public override void Awake()
        {
            
            base.Awake();
        }
    }
    
}