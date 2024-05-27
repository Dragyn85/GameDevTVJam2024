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
        Color defaultColor = nameLable.color;
        
        loginButton.onClick.AddListener(() =>
        {
            string name = nameInputField.text;
            if (!AuthenticationManager.Instance.TryLogIn(name))
            {
                nameLable.color = Color.red;
            }
            else
            {
                nameLable.color = defaultColor;
            }
        });

        offlineButton.onClick.AddListener(() => { AuthenticationManager.Instance.RequestOfflineMode(); });
    }
}