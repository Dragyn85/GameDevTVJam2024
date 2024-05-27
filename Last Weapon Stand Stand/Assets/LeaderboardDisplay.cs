using System;
using System.Threading.Tasks;
using UnityEngine;

public class LeaderboardDisplay : MonoBehaviour
{
    [SerializeField] private HighScoreEntryDisplay _leaderboardDisplayPrefab;

    [ContextMenu("Update Leaderboard")]
    private void UpdateLeadeboard()
    {
        UpdateLeaderboard();
    }

    public async void UpdateLeaderboard()
    {
        if (AuthenticationManager.Instance.IsAuthenticated())
        {
            ClearLeaderboard();
            await InstantiateLeaderboardEntries();
        }
    }
    
    void ClearLeaderboard()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        } 
    }

    async Task InstantiateLeaderboardEntries()
    {
        var entries = await LeaderBoardManager.Instance.GetTopTen();

        foreach (var entry in entries)
        {
            var leaderboardDisplay = Instantiate(_leaderboardDisplayPrefab, transform);
            leaderboardDisplay.SetTexts(entry.PlayerName, entry.Score);
        }
    }
}
