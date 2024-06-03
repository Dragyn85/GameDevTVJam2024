using System;
using System.Threading.Tasks;
using UnityEngine;

public class LeaderboardDisplay : MonoBehaviour
{
    [SerializeField] private HighScoreEntryDisplay _leaderboardDisplayPrefab;
    [SerializeField] private Transform _leaderboardParent;
    [SerializeField] private int maxEntries = 10;
    [SerializeField] private CanvasGroup canvasGroup;
    
    private void OnEnable()
    {
        Debug.Log("LeaderboardDisplay OnEnable");
    }

    private void Start()
    {
        if (AuthenticationManager.Instance.IsAuthenticated())
        {
            LeaderBoardManager.Instance.DisableUpdates(false);
            UpdateLeaderboard();
        }

        LeaderBoardManager.Instance.OnLeaderBoardUpdated += UpdateLeaderboard;
        LeaderBoardManager.Instance.RequestUpdate();
    }

    private void OnDestroy()
    {
        //LeaderBoardManager.Instance.OnLeaderBoardUpdated -= UpdateLeaderboard;
    }

    public void UpdateLeaderboard()
    {
        ClearLeaderboard();
        InstantiateLeaderboardEntries();
    }

    void ClearLeaderboard()
    {
        if (_leaderboardParent)
        {
            foreach (Transform child in _leaderboardParent)
            {
                Destroy(child.gameObject);
            }
        }
    }

    void InstantiateLeaderboardEntries()
    {
        var entries = LeaderBoardManager.Instance.GetLeaderboardEntries();
        int numberOfEntries = 0;
        foreach (var entry in entries)
        {
            var leaderboardDisplay = Instantiate(_leaderboardDisplayPrefab, _leaderboardParent);
            leaderboardDisplay.SetTexts($"No. {numberOfEntries+1} {entry.PlayerName}", entry.Score);
            numberOfEntries++;
            if (numberOfEntries >= maxEntries)
            {
                break;
            }
        }
    }

    public void Hide(bool hide)
    {
        canvasGroup.alpha = hide ? 0 : 1;
        canvasGroup.interactable = !hide;
        canvasGroup.blocksRaycasts = !hide;
        
        LeaderBoardManager.Instance.DisableUpdates(hide);
    }
}