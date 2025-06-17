using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneAndUIManager : MonoBehaviour, IUpdatable
{
    public static SceneAndUIManager Instance;

    private int lives = 3;
    private int paddleHits = 0;
    private int blocksRemaining = 0;
    private int score = 0;

    private TMP_Text livesText;
    private TMP_Text hitsText;
    private TMP_Text blocksText;
    private TMP_Text scoreText;

    public Canvas PauseCanva;

    private bool isInitialized = false;

    private void OnEnable()
    {
        Instance = this;
        TryRegister();
    }

    private void TryRegister()
    {
        if (CustomUpdateManager.Instance != null)
        {
            CustomUpdateManager.Instance.RegisterUpdatable(this);
        }
        else
        {
            StartCoroutine(WaitForUpdateManager());
        }
    }

    private IEnumerator WaitForUpdateManager()
    {
        while (CustomUpdateManager.Instance == null)
            yield return null;

        CustomUpdateManager.Instance.RegisterUpdatable(this);
    }

    private void OnDisable()
    {
        if (CustomUpdateManager.Instance != null)
            CustomUpdateManager.Instance.UnregisterUpdatable(this);
    }

    public void OnUpdate()
    {
        if (!isInitialized)
        {
            Initialize();
            isInitialized = true;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
    }

    private void Initialize()
    {
        livesText = GameObject.Find("LivesText")?.GetComponent<TMP_Text>();
        hitsText = GameObject.Find("HitsText")?.GetComponent<TMP_Text>();
        blocksText = GameObject.Find("BlocksText")?.GetComponent<TMP_Text>();
        scoreText = GameObject.Find("ScoreText")?.GetComponent<TMP_Text>();

        if (PauseCanva != null)
            PauseCanva.gameObject.SetActive(false);

        CountBlocksAtStart();
        UpdateAllUI();
    }

    private void CountBlocksAtStart()
    {
        blocksRemaining = GameObject.FindGameObjectsWithTag("Brick").Length;
    }

    private void UpdateAllUI()
    {
        UpdateLivesUI();
        UpdateHitsUI();
        UpdateBlocksUI();
        UpdateScoreUI();
    }

    public void LoseLife()
    {
        lives--;
        UpdateLivesUI();

        if (lives <= 0)
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("YouLose");
        }
    }

    public void RegisterPaddleHit()
    {
        paddleHits++;
        UpdateHitsUI();
    }

    public void RegisterBrickDestroyed()
    {
        blocksRemaining--;
        UpdateBlocksUI();

        if (blocksRemaining <= 0)
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("YouWin");
        }
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreUI();
    }

    private void UpdateLivesUI()
    {
        if (livesText != null)
            livesText.text = "Vidas: " + lives;
    }

    private void UpdateHitsUI()
    {
        if (hitsText != null)
            hitsText.text = "Golpes: " + paddleHits;
    }

    private void UpdateBlocksUI()
    {
        if (blocksText != null)
            blocksText.text = "Bloques: " + blocksRemaining;
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }

    public void TogglePauseMenu()
    {
        if (PauseCanva == null) return;

        bool isPaused = PauseCanva.gameObject.activeSelf;

        PauseCanva.gameObject.SetActive(!isPaused);
        Time.timeScale = isPaused ? 1f : 0f;
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PlayLevel(string levelName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(levelName);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
