using System;
using System.Threading.Tasks;
using Unity.Android.Gradle.Manifest;
using Unity.Services.Core;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using Action = System.Action;

public class AuthenticationManager : MonoBehaviour
{
    #region Singelton

    private static AuthenticationManager instance;

    public static AuthenticationManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindFirstObjectByType<AuthenticationManager>();
                if (instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(AuthenticationManager).ToString());
                    instance = singletonObject.AddComponent<AuthenticationManager>();
                }
            }
            return instance;
        }
    }

    private async void Awake()
    {
        Debug.Log("Awake AuthenticationManager");
        if (instance == null)
        {

            instance = this;
            DontDestroyOnLoad(gameObject);
            await Initialize();
        }
        else if(Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(this);
            await Initialize();
        }
    }

    private async Task Initialize()
    {
        Debug.Log("Initializing Unity Services");
        try
        {
            await UnityServices.InitializeAsync();
        }
        catch (Exception e)
        {
            if (e is ServicesInitializationException initExeption)
            {
                Debug.LogError("Unity Services Initialization Failed with error: " + initExeption.Message);
            }
            else
            {
                Debug.LogError("Unity project not linked to a project " + e.Message);
            }
        }

        Debug.Log("Unity Services Initialized");
    }

    #endregion

    private bool offlineMode = false;
    
    [FormerlySerializedAs("debugBoard")]
    [Tooltip("This will be inactivated in the build")]
    [SerializeField] bool debugMode = true;

    public event Action OnAuthenticationComplete;
    public bool IsDebugModeActive()
    {
        return debugMode;
    }
    
    public bool IsAuthenticated()
    {
        if(offlineMode)
        {
            return true;
        }
        
        if (ugsAuthentication == null)
        {
            return false;
        }
        
        if (ugsAuthentication.IsAuthenticationValid())
        {
            return true;
        }

        return false;
    }
    
    UGSAuthentication ugsAuthentication;

    public void RequestOfflineMode()
    {
        offlineMode = true;
    }

    public async Task<bool> TryLogIn(string name)
    {
        if (IsValidName(name))
        {
            Debug.Log("TryLogIn");
            if (ugsAuthentication == null)
            {
                ugsAuthentication = new UGSAuthentication();
            }
            
            await ugsAuthentication.AnonymusSignIn(name);
            OnAuthenticationComplete?.Invoke();
            return true;
        }

        return false;
    }

    private bool IsValidName(string name)
    {
        return !string.IsNullOrEmpty(name);
    }

}