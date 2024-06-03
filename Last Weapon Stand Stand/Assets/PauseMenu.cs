using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] Button quitButton;
    [SerializeField] Button MainMenuButton;
    
    [SerializeField] PlayerController playerController;
    
    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.None;
        quitButton.onClick.AddListener(QuitGame);
        MainMenuButton.onClick.AddListener(MainMenu);
        Time.timeScale = 0;
    }

    private void OnDisable()
    {
        quitButton.onClick.RemoveListener(QuitGame);
        MainMenuButton.onClick.RemoveListener(MainMenu);
        Time.timeScale = 1;
    }

    private void QuitGame()
    {
        playerController.Quit();
    }
    
    private void MainMenu()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("Main Menu");
    }
}
