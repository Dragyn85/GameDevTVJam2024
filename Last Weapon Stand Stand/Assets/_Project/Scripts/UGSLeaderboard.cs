using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Leaderboards;
using Unity.Services.Leaderboards.Models;
using UnityEngine;

public class UGSAuthentication
{
    public async void AnonymusSignIn(string playerName)
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

    public bool IsAuthenticationValid()
    {
        return AuthenticationService.Instance.IsAuthorized;
    }
}

public class UGSLeaderboard : ILeaderboard
{
    // Create a leaderboard with this ID in the Unity Cloud Dashboard
    const string LeaderboardId = "Highscore";

    string VersionId { get; set; }
    int Offset { get; set; }
    int Limit { get; set; }
    int RangeLimit { get; set; }
    List<string> FriendIds { get; set; }


    async void Awake()
    {
        await UnityServices.InitializeAsync();
    }


    public async void AddScore(double ScoreToAdd)
    {
        var scoreResponse = await LeaderboardsService.Instance.AddPlayerScoreAsync(LeaderboardId, ScoreToAdd);
        //Debug.Log(JsonConvert.SerializeObject(scoreResponse));
    }


    public async Task<List<LeaderboardEntry>> GetScores()
    {
        if (!AuthenticationManager.Instance.IsAuthenticated())
        {
            return null;
        }

        var scoresResponse =
            await LeaderboardsService.Instance.GetScoresAsync(LeaderboardId);
        var numberOfEntries = Mathf.Max(10,scoresResponse.Results.Count);
        var BestScores = scoresResponse.Results;
        return BestScores;
    }

    public async Task<double> GetPlayerScore()
    {
        var scoreResponse =
            await LeaderboardsService.Instance.GetPlayerScoreAsync(LeaderboardId);
        
        return scoreResponse.Score;
    }
}