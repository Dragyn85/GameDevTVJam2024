using System;
using System.Net.Security;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Leaderboards;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	//[SerializeField] private GameObject SettingPanel;

	[SerializeField] TMP_InputField NameInputField;


	private void Start()
	{
		UGSAuthentication.OnAuthenticationComplete += HandleAutheticationComplete;
	}

	private void HandleAutheticationComplete()
	{
		Debug.Log("Set Name Input Field");
		var PlayerName = AuthenticationService.Instance.PlayerName;
		NameInputField.text = PlayerName;
	}


	public async void UpdatePlayersNameButtonClicked()
	{
		var result = await AuthenticationService.Instance.UpdatePlayerNameAsync(NameInputField.text);
		NameInputField.text = result;
	}
	
	public void PlayButtonClicked()
	{
		SceneManager.LoadScene("Level 1");
	}

	public void SettingsButtonClicked()
	{
		//SettingPanel.SetActive(true);
	}
}
