using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HighScoreEntry
{
    public string name;
    public int score;

    public HighScoreEntry(string name, int score)
    {
        this.name = name;
        this.score = score;
    }
}

public class HighScoreTable : MonoBehaviour
{
    public TMP_Text[] indexTexts;
    public TMP_Text[] nameTexts;
    public TMP_Text[] scoreTexts;

    private List<HighScoreEntry> highScores = new List<HighScoreEntry>();

    public void AddHighScore(string name, int score)
    {
        HighScoreEntry entry = new HighScoreEntry(name, score);
        highScores.Add(entry);
        highScores.Sort((a, b) => b.score.CompareTo(a.score));
        UpdateHighScoreTable();
    }

    private void UpdateHighScoreTable()
    {
        for (int i = 0; i < Mathf.Min(highScores.Count, 10); i++)
        {
            indexTexts[i].text = (i + 1).ToString("00");
            nameTexts[i].text = highScores[i].name;
            scoreTexts[i].text = highScores[i].score.ToString();
        }
    }

    public void ClearTable()
    {
        highScores.Clear();
        UpdateHighScoreTable();
    }
}
