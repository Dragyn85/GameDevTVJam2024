using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using Unity.Services.Leaderboards.Models;

public class LeaderBoardManager : MonoBehaviour
{
    #region Singelton

    public static LeaderBoardManager Instance { get; set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion

    private void Start()
    {
        Initialize();
    }

    ILeaderboard leaderboard;
    
    private void Initialize()
    {
        if (AuthenticationManager.Instance.IsDebugModeActive())
        {
            leaderboard = new DebugLeaderboard();
        }
        else
        {
            leaderboard = new UGSLeaderboard();
        }
    }
    
    

    [SerializeField] private double DebugScore;
    
    
    [ContextMenu("Add Debug Score")]
    public void AddDebugScore()
    {
        leaderboard.AddScore(DebugScore);
    }

    [ContextMenu("Get Scores")]
    public async void GetScores()
    {
        var leaderboardEntries =  await leaderboard.GetScores();

        foreach (var entry in leaderboardEntries)
        {
            Debug.Log($"{entry.PlayerName} Got {entry.Score} points!");
        }
    }

    public async Task<List<LeaderboardEntry>> GetTopTen()
    {
        if(!AuthenticationManager.Instance.IsAuthenticated())
        {
            return null;
        }

        var entries = await leaderboard.GetScores();
        return entries;
    }
}

public interface ILeaderboard
{
    void AddScore(double score);
    Task<List<LeaderboardEntry>> GetScores();
}

public class DebugLeaderboard : ILeaderboard
{
    public void AddScore(double score)
    {
        Debug.Log("Score added: " + score);
    }

    public Task<List<LeaderboardEntry>> GetScores()
    {
        // Create some dummy data for debugging purposes
        var dummyData = new List<LeaderboardEntry>
        {
            new LeaderboardEntry("PlayerID1", "Scat", 1, 100),
            new LeaderboardEntry("PlayerID2", "David", 2, 90),
            new LeaderboardEntry("PlayerID3", "Joppe", 3, 20)
        };

        // Return a completed task with the dummy data
        return Task.FromResult(dummyData);
    }
}
