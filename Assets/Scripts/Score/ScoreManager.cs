using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    private int score = 0;
    private TMP_Text scoreText;
    private int bricksLeft;

    private GameController gameController;
    private void Awake()
    {
        gameController = FindAnyObjectByType<GameController>();
        if (Instance == null)
            Instance = this;

        GameObject textObject = GameObject.Find("ScoreText");
        if (textObject != null)
            scoreText = textObject.GetComponent<TMP_Text>();

        UpdateScoreUI();
        CheckBricks();
        Debug.Log(bricksLeft);
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
        bricksLeft = gameController.BricksToWin;

        if (bricksLeft <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }


        Debug.Log("Bricks left: " + bricksLeft);
    }
}
