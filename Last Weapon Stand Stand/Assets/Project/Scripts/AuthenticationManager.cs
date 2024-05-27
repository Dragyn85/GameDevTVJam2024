using System.Threading.Tasks;
using Unity.Services.Core;
using UnityEditor;
using UnityEngine;

public class AuthenticationManager : MonoBehaviour
{
    #region Singelton

    public static AuthenticationManager Instance { get; private set; }

    private async void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Initialize();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private async void Initialize()
    {
        await UnityServices.InitializeAsync();
    }

    #endregion

    private bool OfflineMode;

    public bool IsAuthenticated()
    {
        if (ugsAuthentication == null)
        {
            return false;
        }
        
        if (ugsAuthentication.IsAuthenticationValid())
        {
            return true;
        }

        return true;
    }
    
    UGSAuthentication ugsAuthentication;

    public void RequestOfflineMode()
    {
        OfflineMode = true;
    }

    public bool TryLogIn(string name)
    {
        if (IsValidName(name))
        {
            if (ugsAuthentication == null)
            {
                ugsAuthentication = new UGSAuthentication();
            }
            
            ugsAuthentication.AnonymusSignIn(name);
            return true;
        }

        return false;
    }

    private bool IsValidName(string name)
    {
        return !string.IsNullOrEmpty(name);
    }
}