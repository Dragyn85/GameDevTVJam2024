    using System;
using TMPro;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    [Header("Main menu settings")]
    [SerializeField] GameObject mainMenuLoadingScreen;
    [SerializeField] string SceneToLoad;
    
    [Header("Start")]
    [SerializeField] ImageAlphaButton startButton;
    [Header("Settings")]
    [SerializeField] ImageAlphaButton settingsButton;
    [SerializeField] GameObject settingsDisplay;
    [Header("Exit")]
    [SerializeField] ImageAlphaButton exitButton;
    [Header("Credits")]
    [SerializeField] ImageAlphaButton creditsButton;
    [SerializeField] GameObject creditsDisplay;

    [Header("Leaderboard")]
    [SerializeField] LeaderboardDisplay leaderBoardDisplay;
    [SerializeField] TMP_InputField NameInputField;
    [SerializeField] Button UpdatePlayersNameButton;
    
    public void Start()
    {
        startButton.onClick.AddListener(HandleStartButtonClicked);
        settingsButton.onClick.AddListener(HandleSettingsButtonClicked);
        exitButton.onClick.AddListener(HandleExitButtonClicked);
        creditsButton.onClick.AddListener(HandleCreditsButtonClicked);
        UpdatePlayersNameButton.onClick.AddListener(UpdatePlayersNameButtonClicked);
        
        if (AuthenticationManager.Instance.IsAuthenticated())
        {
            HandleAutheticationComplete();
        }
        else
        {
            UGSAuthentication.OnAuthenticationComplete += HandleAutheticationComplete;
        }
    }
    
    public async void UpdatePlayersNameButtonClicked()
    {
        var result = await AuthenticationService.Instance.UpdatePlayerNameAsync(NameInputField.text);
        NameInputField.text = result;
        LeaderBoardManager.Instance.RequestUpdate();
    }
    private void HandleAutheticationComplete()
    {
        Debug.Log("Set Name Input Field");
        var PlayerName = AuthenticationService.Instance.PlayerName;
        NameInputField.text = PlayerName;
    }

    private void HandleExitButtonClicked()
    {
        Application.Quit();
    }

    private void HandleSettingsButtonClicked()
    {
        var active = settingsDisplay.activeSelf;
        settingsDisplay.SetActive(!active);
        creditsDisplay.SetActive(false);
        
        leaderBoardDisplay.Hide(!active);
    }

    private void HandleCreditsButtonClicked()
    {
        var active = creditsDisplay.activeSelf;
        creditsDisplay.SetActive(!active);
        settingsDisplay.SetActive(false);

        leaderBoardDisplay.Hide(!active);

    }

    private void HandleStartButtonClicked()
    {
        LeaderBoardManager.Instance.DisableUpdates(true);
        SceneManager.LoadScene(SceneToLoad);
    }
}
