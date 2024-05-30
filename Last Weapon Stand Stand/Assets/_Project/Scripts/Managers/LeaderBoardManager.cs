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

    public static LeaderBoardManager Instance;
    
    private void Awake()
    {
        Debug.Log("Awake LeaderboardManager");
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

    public event Action OnLeaderBoardUpdated;

    private List<LeaderboardEntry> localLeaderboardEntries;
    private double currentLocalHighScore;
    private float nextUpdateTime;
    private float refreshMinInterval = 300;
    private bool isEnabled = true;

    /// <summary>
    /// Adds a score to the leaderboard, if the score is higher than the current local highscore it will update the local highscore.
    /// OnLeaderBoardUpdated event will be invoked after the score has been added.
    /// </summary>
    /// <param name="score"></param>
    public async void AddScore(double score)
    {
        if (score > currentLocalHighScore)
        {
            currentLocalHighScore = score;
            PlayerPrefs.SetFloat("LocalHighScore", (float) score);
            await leaderboard.AddScore(score);
            UpdateLocalEntries();
        }
    }

    public void RequestUpdate()
    {
        nextUpdateTime = Time.time;
    }

    public void DisableUpdates(bool disable)
    {
        isEnabled = !disable;
    }
    


    private void HandleAutheticationComplete()
    {
        Debug.Log("LeaderboardManager: HandleAutheticationComplete");
        Initialize();
        currentLocalHighScore = PlayerPrefs.GetFloat("LocalHighScore", 0);
    }

    ILeaderboard leaderboard;

    private void Initialize()
    {
        Debug.Log("initializing LeaderboardManager");
        if (AuthenticationManager.Instance.IsDebugModeActive())
        {
            leaderboard = new DebugLeaderboard();
        }
        else
        {
            leaderboard = new UgsLeaderboard();
        }

        UpdateLocalEntries();
    }

    async void UpdateLocalEntries()
    {
        if(leaderboard == null)
        {
            return;
        }
        nextUpdateTime = Time.time+refreshMinInterval;
        localLeaderboardEntries = await leaderboard.GetScores();
        
        OnLeaderBoardUpdated?.Invoke();
    }

    public List<LeaderboardEntry> GetLeaderboardEntries()
    {
        if (localLeaderboardEntries == null)
        {
            localLeaderboardEntries = new List<LeaderboardEntry>();
        }

        return localLeaderboardEntries;
    }

    private void Update()
    {
        if (isEnabled && Time.time > nextUpdateTime)
        {
            UpdateLocalEntries();
        }
    }

    private void Start()
    {
        Debug.Log("LeaderboardManager: Start");
        AuthenticationManager.Instance.OnAuthenticationComplete += HandleAutheticationComplete;
        AuthenticationManager.Instance.OnAuthenticationChanged += HandleAutheticationChanged;
    }

    private void HandleAutheticationChanged()
    {
        RequestUpdate();
    }

    private void OnDestroy()
    {
        //AuthenticationManager.Instance.OnAuthenticationChanged -= HandleAutheticationChanged;
        //AuthenticationManager.Instance.OnAuthenticationComplete -= HandleAutheticationComplete;
    }


    #region Debugging

    [SerializeField] private double DebugScore;


    [ContextMenu("Add Debug Score")]
    public void AddDebugScore()
    {
        leaderboard.AddScore(DebugScore);
    }

    [ContextMenu("Get Scores")]
    public async void GetScores()
    {
        var leaderboardEntries = await leaderboard.GetScores();

        foreach (var entry in leaderboardEntries)
        {
            Debug.Log($"{entry.PlayerName} Got {entry.Score} points!");
        }
    }

    #endregion
}

public class DebugLeaderboard : ILeaderboard
{
    int currentID = 4;

    List<LeaderboardEntry> entries = new List<LeaderboardEntry>
    {
        new LeaderboardEntry("PlayerID1", "Scat", 1, 100),
        new LeaderboardEntry("PlayerID2", "David", 2, 90),
        new LeaderboardEntry("PlayerID3", "Joppe", 3, 20)
    };

    public async Task AddScore(double score)
    {
        var newEntry = new LeaderboardEntry($"PlayerID{currentID++}", "Player Name", 1, 100);
        entries.Add(newEntry);
    }

    public Task<List<LeaderboardEntry>> GetScores()
    {
        // Return a completed task with the dummy data
        return Task.FromResult(entries);
    }
}