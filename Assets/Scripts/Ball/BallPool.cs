using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPool : MonoBehaviour
{
    public static BallPool Instance;

    private GameObject ballPrefab;
    private List<GameObject> pool = new List<GameObject>();

    private void Awake()
    {
        ballPrefab = Resources.Load<GameObject>("Prefabs/Ball");
        if (Instance == null)
            Instance = this;
    }

    public GameObject GetBall()
    {
        GameObject ball;

        if (pool.Count > 0)
        {
            ball = pool[0];
            pool.RemoveAt(0);
            ball.SetActive(true);
        }
        else
        {
            ball = Instantiate(ballPrefab);
        }

        BallController ballController = ball.GetComponent<BallController>();
        if (ballController != null)
        {
            ballController.ResetBall();
        }

        return ball;
    }

    public void ReturnBall(GameObject ball)
    {
        CustomUpdateManager.Instance.UnregisterUpdatable(ball.GetComponent<IUpdatable>());
        ball.SetActive(false);
        pool.Add(ball);
    }
}
