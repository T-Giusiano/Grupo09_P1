using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    private int score = 0;
    private TMP_Text scoreText;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        GameObject textObject = GameObject.Find("ScoreText");
        if (textObject != null)
            scoreText = textObject.GetComponent<TMP_Text>();

        UpdateScoreUI();
    }

    public void AddScore(int points)
    {
        score += points;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score.ToString();
    }

    public void CheckBricks()
    {
        GameObject[] bricks = GameObject.FindGameObjectsWithTag("Brick");
        if (bricks.Length == 1)
        {
            WinGame();
        }
    }

    private void WinGame()
    {
        SceneManager.LoadScene("YouWin");
    }
}
