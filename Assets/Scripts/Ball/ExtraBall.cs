using System.Collections.Generic;
using UnityEngine;

public class ExtraBall : MonoBehaviour, IUpdatable
{
    private float speed = 15f;
    private Vector3 velocity;
    private GameObject paddle;
    private List<GameObject> bricks;
    private List<GameObject> powerUps;

    private void Awake()
    {
        paddle = GameObject.Find("Paddle");
        DetectBricks();
        DetectPowerUps();
        LaunchBall();
    }

    private void OnEnable()
    {
        CustomUpdateManager.Instance.RegisterUpdatable(this);
    }

    private void OnDisable()
    {
        CustomUpdateManager.Instance.UnregisterUpdatable(this);
    }

    public void OnUpdate()
    {
        transform.position += velocity * Time.deltaTime;


        if (transform.position.x <= -32f)
        {
            transform.position = new Vector3(-32f, transform.position.y, transform.position.z);
            velocity.x = -velocity.x;
        }

        if (transform.position.x >= 32f)
        {
            transform.position = new Vector3(32f, transform.position.y, transform.position.z);
            velocity.x = -velocity.x;
        }
        if (transform.position.y >= 34f)
        {
            transform.position = new Vector3(transform.position.x, 34f, transform.position.z);
            velocity.y = -velocity.y;
        }

        if (transform.position.y <= -8f)
        {
            Destroy(gameObject);
            return;
        }

        if (IsCollidingWith(paddle))
        {
            velocity.y = Mathf.Abs(velocity.y);
            float hitPoint = (transform.position.x - paddle.transform.position.x) / 1.5f;
            velocity.x = hitPoint * speed;
        }

        CheckCollisions(bricks, true);
        CheckCollisions(powerUps, false);
    }

    private void LaunchBall()
    {
        float angle = Random.Range(-45f, 45f);
        float radians = angle * Mathf.Deg2Rad;
        Vector3 direction = new Vector3(Mathf.Sin(radians), Mathf.Cos(radians), 0f).normalized;
        velocity = direction * speed;
    }

    private void DetectBricks()
    {
        bricks = new List<GameObject>(GameObject.FindGameObjectsWithTag("Brick"));
    }

    private void DetectPowerUps()
    {
        powerUps = new List<GameObject>(GameObject.FindGameObjectsWithTag("BrickPUP"));
    }

    private bool IsCollidingWith(GameObject other)
    {
        if (other == null) return false;

        Vector3 ballPos = transform.position;
        Vector3 ballSize = transform.localScale / 2f;
        Vector3 otherPos = other.transform.position;
        Vector3 otherSize = other.transform.localScale / 2f;

        return (Mathf.Abs(ballPos.x - otherPos.x) < (ballSize.x + otherSize.x)) &&
               (Mathf.Abs(ballPos.y - otherPos.y) < (ballSize.y + otherSize.y));
    }

    private void CheckCollisions(List<GameObject> objects, bool isBrick)
    {
        for (int i = objects.Count - 1; i >= 0; i--)
        {
            GameObject obj = objects[i];
            if (obj == null)
            {
                objects.RemoveAt(i);
                continue;
            }

            if (IsCollidingWith(obj))
            {
                velocity.y = -velocity.y;
                ScoreManager.Instance.AddScore(100);

                if (isBrick)
                    ScoreManager.Instance.CheckBricks();
                else
                {
                    Vector3 dropPos = obj.transform.position;
                    GameObject powerUpDrop = Resources.Load<GameObject>("Prefabs/PowerUp");
                    Instantiate(powerUpDrop, dropPos, Quaternion.identity);
                }

                Destroy(obj);
                objects.RemoveAt(i);
                break;
            }
        }
    }
}
