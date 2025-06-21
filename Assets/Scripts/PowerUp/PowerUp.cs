using UnityEngine;

public class PowerUp : MonoBehaviour, IUpdatable
{
    private float fallSpeed = 3f;
    public PowerUpCFIG config;
    public string powerUpName = "Multiball";
    private GameController gameController;
    void OnEnable()
    {
        CustomUpdateManager.Instance.RegisterUpdatable(this);
        if (gameController == null)
        {
            gameController = GameObject.FindObjectOfType<GameController>();
        }
    }

    void OnDisable()
    {
        if (CustomUpdateManager.Instance != null)
            CustomUpdateManager.Instance.UnregisterUpdatable(this);
    }

    public void OnUpdate()
    {
        if (config == null) return;

        transform.position += Vector3.down * fallSpeed * Time.deltaTime;

        if (transform.position.y <= -8f)
        {
            GameObject.Destroy(gameObject);
        }

        GameObject paddle = gameController.paddleGO;
        if (IsCollidingWith(paddle))
        {
            Vector3 spawnPos = new Vector3(paddle.transform.position.x, paddle.transform.position.y + 0.5f, 0f);
            for (int i = 0; i < config.ballsToSpawn; i++)
            {
                gameController.SpawnExtraBall(spawnPos);
            }
            GameObject.Destroy(gameObject);
        }
    }

    bool IsCollidingWith(GameObject other)
    {
        Vector3 thisPos = transform.position;
        Vector3 thisSize = transform.localScale / 2f;
        Vector3 otherPos = other.transform.position;
        Vector3 otherSize = other.transform.localScale / 2f;

        return (Mathf.Abs(thisPos.x - otherPos.x) < (thisSize.x + otherSize.x)) &&
               (Mathf.Abs(thisPos.y - otherPos.y) < (thisSize.y + otherSize.y));
    }
}
