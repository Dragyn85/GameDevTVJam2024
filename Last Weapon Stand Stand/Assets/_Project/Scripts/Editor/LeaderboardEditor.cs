using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CustomEditor(typeof(LeaderboardDisplay))]
public class LeaderboardEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        if(GUILayout.Button("Update Leaderboard"))
        {
            if (Application.isPlaying)
            {
                ((LeaderboardDisplay)target).UpdateLeaderboard();
            }
        }
        
    }
}
