using System;
using System.Threading.Tasks;
using UnityEngine;

public class LeaderboardDisplay : MonoBehaviour
{
    [SerializeField] private HighScoreEntryDisplay _leaderboardDisplayPrefab;
    [SerializeField] private Transform _leaderboardParent;
    
    private void OnEnable()
    {
        LeaderBoardManager.Instance.OnLeaderBoardUpdated += UpdateLeaderboard;
        LeaderBoardManager.Instance.RequestUpdate();
    }

    private void OnDisable()
    {
        LeaderBoardManager.Instance.OnLeaderBoardUpdated -= UpdateLeaderboard;
    }

    public void UpdateLeaderboard()
    {
        ClearLeaderboard();
        InstantiateLeaderboardEntries();
    }

    void ClearLeaderboard()
    {
        foreach (Transform child in _leaderboardParent)
        {
            Destroy(child.gameObject);
        }
    }

    void InstantiateLeaderboardEntries()
    {
        var entries = LeaderBoardManager.Instance.GetLeaderboardEntries();

        foreach (var entry in entries)
        {
            var leaderboardDisplay = Instantiate(_leaderboardDisplayPrefab, _leaderboardParent);
            leaderboardDisplay.SetTexts(entry.PlayerName, entry.Score);
        }
    }
}