using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private Pool<BallController> ballPool; //BallPool
    private Pool<ExtraBall> extraBallPool; //ExtraBallPool

    [SerializeField] private List<PowerUpCFIG> configs; //Config

    private List<Brick> activeBricks = new List<Brick>();
    public List<Brick> ActiveBricks => activeBricks;

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

        GameObject[] spawnBrick1H = GameObject.FindGameObjectsWithTag("Brick1Spawn");
        GameObject[] spawnBrick2H = GameObject.FindGameObjectsWithTag("Brick2Spawn");
        GameObject[] spawnBrickND = GameObject.FindGameObjectsWithTag("Brick3Spawn");
        GameObject[] spawnBrickPUP = GameObject.FindGameObjectsWithTag("Brick4Spawn");

        foreach (GameObject spawnPoint in spawnBrick1H)
        {
            GameObject brickGO = Instantiate(brickPrefab, spawnPoint.transform.position, Quaternion.identity);
            Brick brick = new Brick(brickGO, Brick.BrickType.OneHit, this);
            brick.UpdateVisual();
            activeBricks.Add(brick);
        }
        foreach (GameObject spawnPoint in spawnBrick2H)
        {
            GameObject brickGO = Instantiate(brickPrefab, spawnPoint.transform.position, Quaternion.identity);
            Brick brick = new Brick(brickGO, Brick.BrickType.TwoHit, this);
            brick.UpdateVisual();
            activeBricks.Add(brick);
        }
        foreach (GameObject spawnPoint in spawnBrickND)
        {
            GameObject brickGO = Instantiate(brickPrefab, spawnPoint.transform.position, Quaternion.identity);
            Brick brick = new Brick(brickGO, Brick.BrickType.NonDestructible, this);
            brick.UpdateVisual();
            activeBricks.Add(brick);
        }
        foreach (GameObject spawnPoint in spawnBrickPUP)
        {
            GameObject brickGO = Instantiate(brickPrefab, spawnPoint.transform.position, Quaternion.identity);
            Brick brick = new Brick(brickGO, Brick.BrickType.PowerUp, this);
            brick.UpdateVisual();
            activeBricks.Add(brick);
        }
    }
    public void RemoveBrick(Brick brick)
    {
        activeBricks.Remove(brick);
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
