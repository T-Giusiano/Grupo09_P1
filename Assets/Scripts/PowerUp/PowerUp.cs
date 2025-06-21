using UnityEngine;

public class PowerUp : IUpdatable
{
    private GameObject powerUpObject;
    private float fallSpeed = 3f;
    public PowerUpCFIG config;
    public string powerUpName = "Multiball";
    private GameController gameController;

    public PowerUp(GameObject instance, PowerUpCFIG config, string name, GameController controller)
    {
        this.powerUpObject = instance;
        this.config = config;
        this.powerUpName = name;
        this.gameController = controller;

        CustomUpdateManager.Instance.RegisterUpdatable(this);
    }

    public void OnUpdate()
    {
        if (config == null) return;

        powerUpObject.transform.position += Vector3.down * fallSpeed * Time.deltaTime;

        if (powerUpObject.transform.position.y <= -8f)
        {
            GameObject.Destroy(powerUpObject);
            CustomUpdateManager.Instance.UnregisterUpdatable(this);
            return;
        }

        GameObject paddle = gameController.paddleGO;
        if (IsCollidingWith(paddle))
        {
            Vector3 spawnPos = new Vector3(paddle.transform.position.x, paddle.transform.position.y + 0.5f, 0f);
            for (int i = 0; i < config.ballsToSpawn; i++)
            {
                gameController.SpawnExtraBall(spawnPos);
            }

            GameObject.Destroy(powerUpObject);
            CustomUpdateManager.Instance.UnregisterUpdatable(this);
        }
    }

    private bool IsCollidingWith(GameObject other)
    {
        Vector3 thisPos = powerUpObject.transform.position;
        Vector3 thisSize = powerUpObject.transform.localScale / 2f;
        Vector3 otherPos = other.transform.position;
        Vector3 otherSize = other.transform.localScale / 2f;

        return (Mathf.Abs(thisPos.x - otherPos.x) < (thisSize.x + otherSize.x)) &&
               (Mathf.Abs(thisPos.y - otherPos.y) < (thisSize.y + otherSize.y));
    }
}
