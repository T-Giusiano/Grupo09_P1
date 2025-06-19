using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class GameController : MonoBehaviour
{
    private GameObjectPool ballPool;
    private GameObjectPool extraBallPool;

    [SerializeField] private List<PowerUpCFIG> configs;

    private List<Brick> activeBricks = new List<Brick>();
    private int bricksToWin = 0;
    public List<Brick> ActiveBricks => activeBricks;
    public int BricksToWin => bricksToWin;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip bounceClip;

    private List<IUpdatable> activeBalls = new List<IUpdatable>();

    [SerializeField] private List<LevelConfig> levelConfigs;

    [SerializeField] private int currentLevel = 0;

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
        Transform levelParent = GameObject.Find("Level").transform;
        Transform[] spawns = levelParent.GetComponentsInChildren<Transform>();

        int[] levelData = levelConfigs[currentLevel].bricks;

        if (levelData.Length != spawns.Length - 1)
        {
            Debug.LogError("LevelData y cantidad de spawn points no coinciden.");
            return;
        }

        for (int i = 1; i < spawns.Length; i++)
        {
            int type = levelData[i - 1];
            if (type == 4)
                continue;

            CrearBrick(spawns[i].position, (Brick.BrickType)type);

            if (type != 2)
            {
                bricksToWin++;
            }
        }
    }

    private void CrearBrick(Vector3 pos, Brick.BrickType type)
    {
        var prefab = Resources.Load<GameObject>("Prefabs/Brick");
        var go = Instantiate(prefab, pos, Quaternion.identity);
        var brick = new Brick(go, type, this);
        brick.UpdateVisual();
        activeBricks.Add(brick);
    }

    public void RemoveBrick(Brick brick)
    {
        activeBricks.Remove(brick);
        bricksToWin--;
        ScoreManager.Instance.CheckBricks();
    }

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
