using Firebase.Auth;
using UnityEngine;

namespace Kit.Services.Auth
{
    public class AuthManager : SingletonBehaviour<AuthManager>
    {


        private void Test()
        { 
            var instance = Firebase.Auth.FirebaseAuth.DefaultInstance; 
            var provider = GoogleAuthProvider.GetCredential("a", "b"); 
       
        }
    }
    
    
}