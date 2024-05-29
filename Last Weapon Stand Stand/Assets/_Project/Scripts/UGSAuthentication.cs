using System.Threading.Tasks;
using Unity.Services.Authentication;
using UnityEngine;

public class UGSAuthentication
{
    public async Task AnonymusSignIn(string playerName)
    {
        await SignInAnonymously(playerName);
    }

    private async Task SignInAnonymously(string playerName)
    {
        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in as: " + AuthenticationService.Instance.PlayerId);
        };
        AuthenticationService.Instance.SignInFailed += s =>
        {
            // Take some action here...
            Debug.Log(s);
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        await AuthenticationService.Instance.UpdatePlayerNameAsync(playerName);
    }
    public async Task UpdatePLayerName(string playerName)
    {
        await AuthenticationService.Instance.UpdatePlayerNameAsync(playerName);
    }

    public bool IsAuthenticationValid()
    {
        return AuthenticationService.Instance.IsAuthorized;
    }
}