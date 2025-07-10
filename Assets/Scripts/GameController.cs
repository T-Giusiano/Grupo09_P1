using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System;
using UnityEditor;


public class GameController : MonoBehaviour
{
    private GameObjectPool ballPool;
    private GameObjectPool extraBallPool;

    //ReferenciasPaddleManager
    private GameObject paddlePrefab;
    public GameObject paddleGO;
    [SerializeField] private PaddleManager paddleManager;
    [SerializeField] private string[] parallaxAddresses;
    [SerializeField] private float[] parallaxFactors;
    [SerializeField] private Transform parallaxParent;
    [SerializeField] private string brickAdress;

    [SerializeField] private List<PowerUpCFIG> configs;

    private List<Brick> activeBricks = new List<Brick>();
    private int bricksToWin = 0;
    public List<Brick> ActiveBricks => activeBricks;
    public int BricksToWin => bricksToWin;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip bounceClip;
    [SerializeField] AssetReferenceGameObject level;

    private List<IUpdatable> activeBalls = new List<IUpdatable>();

    [SerializeField] private List<LevelConfig> levelConfigs;

    [SerializeField] private int currentLevel = 1;


    private void Awake()
    {
        level.LoadAssetAsync().Completed += OnAddressableLoaded;
        PUPManager.Instance.Initialize(configs);

        //Referencias de prefabs
        GameObject ballPrefab = Resources.Load<GameObject>("Prefabs/Ball");
        GameObject extraBallPrefab = Resources.Load<GameObject>("Prefabs/ExtraBall");
        paddlePrefab = Resources.Load<GameObject>("Prefabs/Paddle");
        GameObject[] marginsPrefab = GameObject.FindGameObjectsWithTag("Margin");

        //Inicio de Pools
        ballPool = new GameObjectPool(ballPrefab, 3, null);
        extraBallPool = new GameObjectPool(extraBallPrefab, 6, null);

        //Instanciar Paddle y Ball principal
        SpawnPaddle(new Vector3(0, 0, 0));
        SpawnInitialBall();

        //Set de colores de prefabs
        ColorController.SetColorByTag(paddlePrefab);
        foreach (GameObject margin in marginsPrefab)
        {
            ColorController.SetColorByTag(margin);
        }
    }
    private void OnAddressableLoaded(AsyncOperationHandle<GameObject> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            GameObject levelInstance = Instantiate(handle.Result);
            levelInstance.SetActive(true);


            Transform levelParent = levelInstance.transform;
            SpawnBricks(levelParent);
        }
        else
        {
            Debug.LogError("Loading Asset Failed");
        }
    }
    private void Update()
    {
        for (int i = activeBalls.Count - 1; i >= 0; i--)
        {
            activeBalls[i].OnUpdate();
        }
    }

    private void SpawnBricks(Transform levelParent)
    {
        Transform[] spawns = levelParent.GetComponentsInChildren<Transform>(true);
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
        SceneAndUIManager.Instance.UpdateBlocksUI();
        Debug.Log($"Se generaron {bricksToWin} ladrillos.");
    }

    private void CrearBrick(Vector3 pos, Brick.BrickType type)
    {
        Addressables.LoadAssetAsync<GameObject>(brickAdress).Completed += (handle) =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                GameObject go = Instantiate(handle.Result, pos, Quaternion.identity);
                var brick = new Brick(go, type, this);

                string materialKey = "MaterialPropertyBlock";
                Addressables.LoadAssetAsync<Material>(materialKey).Completed += (matHandle) =>
                {
                    if (matHandle.Status == AsyncOperationStatus.Succeeded)
                    {
                        var renderer = go.GetComponentInChildren<MeshRenderer>();
                        renderer.material = matHandle.Result;
                    }
                    else
                    {
                        Debug.LogError($"error en material {materialKey}");
                    }
                };

                brick.UpdateVisual();
                activeBricks.Add(brick);
            }
            else
            {
                Debug.LogError("No se pudo cargar el brickprefab");
            }
        };
    }


    public void RemoveBrick(Brick brick)
    {
        activeBricks.Remove(brick);
        bricksToWin--;
        SceneAndUIManager.Instance.CheckBricks();
        SceneAndUIManager.Instance.UpdateBlocksUI();
    }
    private void SpawnPaddle(Vector3 position)
    {
        paddleGO = Instantiate(paddlePrefab, position, Quaternion.identity);
        ColorController.SetColorByTag(paddleGO);

        paddleManager = new PaddleManager(paddleGO, parallaxAddresses, parallaxFactors, parallaxParent);
    }

    private void SpawnInitialBall()
    {
        GameObject ballGO = ballPool.Get();
        BallController ball = new BallController(ballGO, audioSource, bounceClip, this, paddleGO);
        ball.GameObject.SetActive(true);
        ball.ResetBall(paddleGO);

        activeBalls.Add(ball);
        ColorController.SetColorByTag(ballGO);
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
        ExtraBall ball = new ExtraBall(ballGO, audioSource, bounceClip, this, paddleGO);
        ball.LaunchBall();

        activeBalls.Add(ball);
        ColorController.SetColorByTag(ballGO);
    }
    public void SpawnPowerUp(Vector3 position, string powerUpName)
    {
        GameObject prefab = Resources.Load<GameObject>("Prefabs/PowerUp");
        GameObject instance = Instantiate(prefab, position, Quaternion.identity);

        PowerUpCFIG config = PUPManager.Instance.GetConfig(powerUpName);
        if (config == null)
        {
            Debug.LogError("No se encontró config para " + powerUpName);
            return;
        }

        PowerUp newPowerUp = new PowerUp(instance, config, powerUpName, this);

        PUPManager.Instance.RegisterPowerUp(powerUpName);
        ColorController.SetColorByTag(instance);
    }

    public void ReturnExtraBallToPool(ExtraBall ball)
    {
        ball.GameObject.SetActive(false);
        activeBalls.Remove(ball);
        extraBallPool.ReturnToPool(ball.GameObject);
    }
}
