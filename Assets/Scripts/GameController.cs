using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [Header("Ball Pools")]
    private Pool<BallController> ballPool;
    private Pool<ExtraBall> extraBallPool;

    [Header("Bricks")]
    private List<GameObject> activeBricks = new List<GameObject>();

    [Header("Configs")]
    [SerializeField] private List<PowerUpCFIG> configs;

    private void Awake()
    {
        // Inicializar PowerUp configs
        PUPManager.Instance.Initialize(configs);

        // Cargar prefabs desde Resources
        BallController ballPrefab = Resources.Load<BallController>("Prefabs/Ball");
        ExtraBall extraBallPrefab = Resources.Load<ExtraBall>("Prefabs/ExtraBall");

        // Crear pools
        ballPool = new Pool<BallController>(ballPrefab, 3, null);
        extraBallPool = new Pool<ExtraBall>(extraBallPrefab, 6, null);

        SpawnBricks();
    }

    private void Update()
    {
        CustomUpdateManager.Instance.CustomUpdate();
    }

    private void SpawnBricks()
    {
        GameObject brickPrefab = Resources.Load<GameObject>("Prefabs/Brick");
        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("BrickSpawn");

        foreach (GameObject spawnPoint in spawnPoints)
        {
            GameObject brick = Instantiate(brickPrefab, spawnPoint.transform.position, Quaternion.identity);
            activeBricks.Add(brick);
        }
    }

    private void SpawnInitialBall()
    {
        BallController ball = ballPool.Get();
        ball.gameObject.SetActive(true);
        ball.ResetBall();
    }

    public void ReturnBallToPool(BallController ball)
    {
        ballPool.ReturnToPool(ball);
        SpawnInitialBall();
    }

    public void ReturnExtraBallToPool(ExtraBall ball)
    {
        extraBallPool.ReturnToPool(ball);
    }

    public void SpawnExtraBall(Vector3 position)
    {
        ExtraBall ball = extraBallPool.Get();
        ball.transform.position = position;
        ball.gameObject.SetActive(true);
    }
}
