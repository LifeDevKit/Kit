using System.Threading;
using Cysharp.Threading.Tasks;

namespace Kit
{ 
    public interface ITransition
    {  
        UniTask<bool> ShouldTransition(); 
    }
}