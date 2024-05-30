using System;
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
    [SerializeField] Button startButton;
    [Header("Settings")]
    [SerializeField] Button settingsButton;
    [SerializeField] GameObject settingsDisplay;
    [Header("Exit")]
    [SerializeField] Button exitButton;
    [Header("Credits")]
    [SerializeField] Button creditsButton;
    [SerializeField] GameObject creditsDisplay;

    [Header("Leaderboard")]
    [SerializeField] LeaderboardDisplay leaderBoardDisplay;
    
    
    public void Start()
    {
        Invoke(nameof(DisableLoadScene),1f);
        startButton.onClick.AddListener(HandleStartButtonClicked);
        settingsButton.onClick.AddListener(HandleSettingsButtonClicked);
        exitButton.onClick.AddListener(HandleExitButtonClicked);
        creditsButton.onClick.AddListener(HandleCreditsButtonClicked);
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

    void DisableLoadScene()
    {
        mainMenuLoadingScreen.SetActive(false);
    }
}
