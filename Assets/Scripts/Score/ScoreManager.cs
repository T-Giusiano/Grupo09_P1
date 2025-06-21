public static class ScoreManager
{
    private static int score = 0;

    public static int Score => score;

    public static void ResetScore()
    {
        score = 0;
    }

    public static void AddScore(int points)
    {
        score += points;
        SceneAndUIManager.Instance.UpdateScoreUI();
    }
}
