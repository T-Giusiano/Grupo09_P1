using System.Collections.Generic;
using UnityEngine;

public class ExtraBall : MonoBehaviour, IUpdatable
{
    private float speed = 15f;
    private Vector3 velocity;
    private GameObject paddle;
    [SerializeField] private AudioClip bounceClip;
    [SerializeField] private AudioSource audioSource;
    private PowerUpCFIG multiballConfig;

    private GameController gameController;

    private void Awake()
    {
        multiballConfig = Resources.Load<PowerUpCFIG>("Configs/PowerUpConfig");
        paddle = GameObject.Find("Paddle");
        gameController = FindObjectOfType<GameController>();
    }

    private void OnEnable()
    {
        CustomUpdateManager.Instance.RegisterUpdatable(this);
        LaunchBall();
    }

    private void OnDisable()
    {
        if (CustomUpdateManager.Instance != null)
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
            gameController.ReturnExtraBallToPool(this);
            return;
        }

        if (IsCollidingWith(paddle))
        {
            velocity.y = Mathf.Abs(velocity.y);
            float hitPoint = (transform.position.x - paddle.transform.position.x) / 1.5f;
            velocity.x = hitPoint * speed;

            if (bounceClip != null && audioSource != null)
                audioSource.PlayOneShot(bounceClip);
        }

        CheckCollisions();
    }

    private void LaunchBall()
    {
        float angle = Random.Range(-45f, 45f);
        float radians = angle * Mathf.Deg2Rad;
        Vector3 direction = new Vector3(Mathf.Sin(radians), Mathf.Cos(radians), 0f).normalized;
        velocity = direction * speed;
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

    private void CheckCollisions()
    {
        var bricksList = gameController.ActiveBricks;

        for (int i = bricksList.Count - 1; i >= 0; i--)
        {
            Brick brick = bricksList[i];
            if (brick == null || brick.BrickObject == null)
            {
                bricksList.RemoveAt(i);
                continue;
            }

            if (IsCollidingWith(brick.BrickObject))
            {
                velocity.y = -velocity.y;
                brick.OnBallHit();
                break;
            }
        }

    }

}
