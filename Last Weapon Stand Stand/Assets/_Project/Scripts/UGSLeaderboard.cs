using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Leaderboards;
using Unity.Services.Leaderboards.Models;
using UnityEngine;

public class UgsLeaderboard : ILeaderboard
{
    // Create a leaderboard with this ID in the Unity Cloud Dashboard
    const string LeaderboardId = "Highscore";
    
    async void Awake()
    {
        await UnityServices.InitializeAsync();
    }


    public async Task AddScore(double ScoreToAdd)
    {
        await LeaderboardsService.Instance.AddPlayerScoreAsync(LeaderboardId, ScoreToAdd);
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
        return scoresResponse.Results;
        
    }

    public async Task<double> GetPlayerScore()
    {
        var scoreResponse =
            await LeaderboardsService.Instance.GetPlayerScoreAsync(LeaderboardId);
        
        return scoreResponse.Score;
    }
}