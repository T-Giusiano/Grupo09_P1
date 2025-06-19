using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private GameObjectPool ballPool;
    private GameObjectPool extraBallPool;

    [SerializeField] private List<PowerUpCFIG> configs;

    private List<Brick> activeBricks = new List<Brick>();
    public List<Brick> ActiveBricks => activeBricks;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip bounceClip;

    private List<IUpdatable> activeBalls = new List<IUpdatable>();

    private void Awake()
    {
        PUPManager.Instance.Initialize(configs);

        GameObject ballPrefab = Resources.Load<GameObject>("Prefabs/Ball");
        GameObject extraBallPrefab = Resources.Load<GameObject>("Prefabs/ExtraBall");

        ballPool = new GameObjectPool(ballPrefab, 3, null);
        extraBallPool = new GameObjectPool(extraBallPrefab, 6, null);

        SpawnInitialBall();
        SpawnBricks();
    }

    private void Update()
    {
        for (int i = activeBalls.Count - 1; i >= 0; i--)
        {
            activeBalls[i].OnUpdate();
        }
    }

    private void SpawnBricks()
    {
        GameObject brickPrefab = Resources.Load<GameObject>("Prefabs/Brick");

        foreach (var spawn in GameObject.FindGameObjectsWithTag("Brick1Spawn"))
            CrearBrick(spawn.transform.position, Brick.BrickType.OneHit);
        foreach (var spawn in GameObject.FindGameObjectsWithTag("Brick2Spawn"))
            CrearBrick(spawn.transform.position, Brick.BrickType.TwoHit);
        foreach (var spawn in GameObject.FindGameObjectsWithTag("Brick3Spawn"))
            CrearBrick(spawn.transform.position, Brick.BrickType.NonDestructible);
        foreach (var spawn in GameObject.FindGameObjectsWithTag("Brick4Spawn"))
            CrearBrick(spawn.transform.position, Brick.BrickType.PowerUp);
    }

    private void CrearBrick(Vector3 pos, Brick.BrickType type)
    {
        var prefab = Resources.Load<GameObject>("Prefabs/Brick");
        var go = Instantiate(prefab, pos, Quaternion.identity);
        var brick = new Brick(go, type, this);
        brick.UpdateVisual();
        activeBricks.Add(brick);
    }

    public void RemoveBrick(Brick brick) => activeBricks.Remove(brick);

    private void SpawnInitialBall()
    {
        GameObject ballGO = ballPool.Get();

        BallController ball = new BallController(ballGO, audioSource, bounceClip, this);
        ball.GameObject.SetActive(true);
        ball.ResetBall();

        activeBalls.Add(ball);
    }

    public void ReturnBallToPool(BallController ball)
    {
        ball.GameObject.SetActive(false);
        activeBalls.Remove(ball);
        ballPool.ReturnToPool(ball.GameObject);

        SpawnInitialBall();
    }

    public void SpawnExtraBall(Vector3 position)
    {
        GameObject ballGO = extraBallPool.Get();
        ballGO.transform.position = position;
        ballGO.SetActive(true);

        ExtraBall ball = new ExtraBall(ballGO, audioSource, bounceClip, this);
        ball.LaunchBall();

        activeBalls.Add(ball);
    }

    public void ReturnExtraBallToPool(ExtraBall ball)
    {
        ball.GameObject.SetActive(false);
        activeBalls.Remove(ball);
        extraBallPool.ReturnToPool(ball.GameObject);
    }
}
