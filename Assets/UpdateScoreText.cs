using TMPro;
using UnityEngine;

public class UpdateScoreText : MonoBehaviour
{
    private void Start()
    {
        TMP_Text scoreText = GetComponent<TMP_Text>();
        if (scoreText != null)
        {
            scoreText.text = $"You Lost!\nYour tower was {GameManager.Score} blocks high.";
        }
    }
}
