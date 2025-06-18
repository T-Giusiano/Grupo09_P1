using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private Pool<BallController> ballPool; //BallPool
    private Pool<ExtraBall> extraBallPool; //ExtraBallPool

    [SerializeField] private List<PowerUpCFIG> configs; //Config

    private List<GameObject> activeBricks = new List<GameObject>();
    public List<GameObject> ActiveBricks => activeBricks;

    [SerializeField] private int powerUpBricksCount;


    private void Awake()
    {
        PUPManager.Instance.Initialize(configs);

        BallController ballPrefab = Resources.Load<BallController>("Prefabs/Ball");
        ExtraBall extraBallPrefab = Resources.Load<ExtraBall>("Prefabs/ExtraBall");

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
        GameObject[] spawnBrick1H = GameObject.FindGameObjectsWithTag("Brick1Spawn"); // 1 hit brick
        GameObject[] spawnBrick2H = GameObject.FindGameObjectsWithTag("Brick2Spawn"); // 2 hit brick
        GameObject[] spawnBrickND = GameObject.FindGameObjectsWithTag("Brick3Spawn"); // no hit brick
        GameObject[] spawnBrickPUP = GameObject.FindGameObjectsWithTag("Brick4Spawn"); // Power Up brick

        foreach (GameObject spawnPoint in spawnBrick1H)
        {
            GameObject brick = Instantiate(brickPrefab, spawnPoint.transform.position, Quaternion.identity);
            brick.tag = "Brick1H";
            activeBricks.Add(brick);
        }
        foreach (GameObject spawnPoint in spawnBrick2H)
        {
            GameObject brick = Instantiate(brickPrefab, spawnPoint.transform.position, Quaternion.identity);
            brick.tag = "Brick2H";
            activeBricks.Add(brick);
        }
        foreach (GameObject spawnPoint in spawnBrickND)
        {
            GameObject brick = Instantiate(brickPrefab, spawnPoint.transform.position, Quaternion.identity);
            brick.tag = "BrickND";
            activeBricks.Add(brick);
        }
        foreach (GameObject spawnPoint in spawnBrickPUP)
        {
            GameObject brick = Instantiate(brickPrefab, spawnPoint.transform.position, Quaternion.identity);
            brick.tag = "BrickPUP";
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
