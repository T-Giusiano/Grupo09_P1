using UnityEngine;

public class Brick
{
    public enum BrickType { OneHit, TwoHit, NonDestructible, PowerUp }
    public BrickType Type { get; private set; }
    public int HitsRemaining { get; private set; }
    public GameObject BrickObject { get; private set; }

    private GameController gameController;

    public Brick(GameObject obj, BrickType type, GameController controller)
    {
        BrickObject = obj;
        Type = type;
        gameController = controller;

        switch (Type)
        {
            case BrickType.OneHit:
                HitsRemaining = 1;
                break;
            case BrickType.TwoHit:
                HitsRemaining = 2;
                break;
            case BrickType.NonDestructible:
                HitsRemaining = int.MaxValue;
                break;
            case BrickType.PowerUp:
                HitsRemaining = 1;
                break;
        }
    }

    public void OnBallHit()
    {
        HitsRemaining--;

        if (HitsRemaining <= 0)
        {
            HandleDestruction();
        }
        else
        {
            UpdateVisual();
        }
    }

    private void HandleDestruction()
    {
        switch (Type)
        {
            case BrickType.OneHit:
                ScoreManager.Instance.AddScore(100);
                ScoreManager.Instance.CheckBricks();
                break;

            case BrickType.TwoHit:
                ScoreManager.Instance.AddScore(150);
                ScoreManager.Instance.CheckBricks();
                break;

            case BrickType.PowerUp:
                if (PUPManager.Instance.CanSpawnPowerUp("Multiball"))
                {
                    Vector3 dropPos = BrickObject.transform.position;
                    GameObject powerUpDrop = Resources.Load<GameObject>("Prefabs/PowerUp");
                    GameObject powerUpInstance = Object.Instantiate(powerUpDrop, dropPos, Quaternion.identity);

                    PowerUpCFIG multiballConfig = Resources.Load<PowerUpCFIG>("Configs/PowerUpConfig");
                    PowerUp powerUpScript = powerUpInstance.GetComponent<PowerUp>();
                    powerUpScript.config = multiballConfig;
                    powerUpScript.powerUpName = "Multiball";

                    PUPManager.Instance.RegisterPowerUp("Multiball");
                }
                break;
        }

        gameController.RemoveBrick(this);
        Object.Destroy(BrickObject);
    }

    public void UpdateVisual()
    {
        Renderer renderer = BrickObject.GetComponent<Renderer>();
        if (renderer == null) return;

        MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
        renderer.GetPropertyBlock(propertyBlock);

        Color newColor = Color.white;

        switch (Type)
        {
            case BrickType.OneHit:
                newColor = Color.red;
                break;

            case BrickType.TwoHit:
                if (HitsRemaining == 2)
                    newColor = Color.yellow;
                else if (HitsRemaining == 1)
                    newColor = Color.red;
                break;

            case BrickType.NonDestructible:
                newColor = Color.gray;
                break;

            case BrickType.PowerUp:
                newColor = Color.green;
                break;
        }

        propertyBlock.SetColor("_Color", newColor);
        renderer.SetPropertyBlock(propertyBlock);
    }
}
