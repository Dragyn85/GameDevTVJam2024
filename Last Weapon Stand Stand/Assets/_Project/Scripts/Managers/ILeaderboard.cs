using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Leaderboards.Models;

public interface ILeaderboard
{
    Task AddScore(double score);
    Task<List<LeaderboardEntry>> GetScores();
}