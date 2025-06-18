using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private Pool<BallController> ballPool; //BallPool
    private Pool<ExtraBall> extraBallPool; //ExtraBallPool

    private List<GameObject> activeBricks = new List<GameObject>(); //BrickPlaces

    [SerializeField] private List<PowerUpCFIG> configs; //Config
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
        AssignPowerUpTags(powerUpBricksCount);
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
    private void AssignPowerUpTags(int count)
    {
        List<GameObject> shuffledBricks = new List<GameObject>(activeBricks);
        for (int i = 0; i < shuffledBricks.Count; i++)
        {
            int randomIndex = Random.Range(i, shuffledBricks.Count);
            (shuffledBricks[i], shuffledBricks[randomIndex]) = (shuffledBricks[randomIndex], shuffledBricks[i]);
        }

        for (int i = 0; i < count && i < shuffledBricks.Count; i++)
        {
            shuffledBricks[i].tag = "BrickPUP";
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
