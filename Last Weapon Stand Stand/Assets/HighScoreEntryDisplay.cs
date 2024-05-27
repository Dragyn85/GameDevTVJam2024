using TMPro;
using UnityEngine;

public class HighScoreEntryDisplay : MonoBehaviour
{
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text scoreText;
    
    public void SetTexts(string name, double score)
    {
        nameText.SetText(name);
        scoreText.SetText(score.ToString("F0"));
    }
}
