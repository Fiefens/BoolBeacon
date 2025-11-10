using TMPro;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class ScoreEntry
{
    public string name;
    public int score;
}

[Serializable]
public class ScoreList
{
    public List<ScoreEntry> scores = new();
}

public class UpdateScoreText : MonoBehaviour
{
    private const string HighScoresKey = "HighScores";

    private void Start()
    {
        string playerName = GameManager.Name;
        int playerScore = GameManager.Score;

        ScoreList scoreList = LoadScores();

        scoreList.scores.Add(new ScoreEntry { name = playerName, score = playerScore });

        scoreList.scores = scoreList.scores
            .OrderByDescending(s => s.score)
            .Take(5)
            .ToList();

        SaveScores(scoreList);

        TMP_Text scoreText = GetComponent<TMP_Text>();
        if (scoreText != null)
        {
            string display = $"You Lost!\nYour tower was {playerScore} blocks high.\n\nHigh Scores:\n";
            foreach (var entry in scoreList.scores)
            {
                display += $"{entry.name}: {entry.score}\n";
            }
            scoreText.text = display.TrimEnd();
        }
    }

    private ScoreList LoadScores()
    {
        if (PlayerPrefs.HasKey(HighScoresKey))
        {
            string json = PlayerPrefs.GetString(HighScoresKey);
            return JsonUtility.FromJson<ScoreList>(json) ?? new ScoreList();
        }
        return new ScoreList();
    }

    private void SaveScores(ScoreList scoreList)
    {
        string json = JsonUtility.ToJson(scoreList);
        PlayerPrefs.SetString(HighScoresKey, json);
        PlayerPrefs.Save();
    }
}