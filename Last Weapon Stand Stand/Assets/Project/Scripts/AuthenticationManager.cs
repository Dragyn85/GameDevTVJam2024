using System;
using System.Threading.Tasks;
using Unity.Services.Core;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

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
            await Initialize();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private async Task Initialize()
    {
        await UnityServices.InitializeAsync();
        Debug.Log("Unity Services Initialized");
    }

    #endregion

    private bool offlineMode;
    
    [FormerlySerializedAs("debugBoard")]
    [Tooltip("This will be inactivated in the build")]
    [SerializeField] bool debugMode = true;

    public bool IsDebugModeActive()
    {
        return debugMode;
    }
    
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

        return offlineMode;
    }
    
    UGSAuthentication ugsAuthentication;

    public void RequestOfflineMode()
    {
        offlineMode = true;
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