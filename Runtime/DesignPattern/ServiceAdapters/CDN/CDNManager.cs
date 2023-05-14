using Cysharp.Threading.Tasks;

namespace Kit.Services.CDN
{ 
    public interface ICDNConfig
    {
        
    } 
    public interface ICDNAccessor
    {
        void Initialize(ICDNConfig config);
        UniTask<bool> HealthCheck();
        
        UniTask DownloadFile(string path);
        UniTask DownloadFolder(string dir);
        
        UniTask Upload(string path);
        UniTask UploadFolder(string directory); 
    }

    public interface IAmazonS3CDNAceessor : ICDNAccessor
    {
        
    }
    
    public class CDNManager
    {
        public ICDNAccessor Accessor; 
    }
}