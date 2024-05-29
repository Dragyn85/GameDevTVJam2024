using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class LoginPanel : MonoBehaviour
{
    [SerializeField] private Button loginButton;
    [SerializeField] private Button offlineButton;
    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private TMP_Text nameLable;


    private void Start()
    {
        loginButton.onClick.AddListener(HandleLoginButtonClicked);

        offlineButton.onClick.AddListener(() => { AuthenticationManager.Instance.RequestOfflineMode(); });
    }

    async void HandleLoginButtonClicked()
    {
        Color defaultColor = nameLable.color;
        string name = nameInputField.text;
        bool loginSuccess = await AuthenticationManager.Instance.TryLogIn(name);
        if (!loginSuccess)
        {
            nameLable.color = Color.red;
        }
        else
        {
            nameLable.color = defaultColor;
        }
    }
}