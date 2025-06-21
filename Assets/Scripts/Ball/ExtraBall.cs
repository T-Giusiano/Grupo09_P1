using UnityEngine;

public class ExtraBall : IUpdatable
{
    public GameObject GameObject { get; private set; }
    public float speed = 15f;
    private Vector3 velocity;
    private GameObject paddle;
    private GameController gameController;
    private AudioSource audioSource;
    private AudioClip bounceClip;

    public ExtraBall(GameObject gameObject, AudioSource audioSource, AudioClip bounceClip, GameController controller, GameObject paddleOBJ)
    {
        this.GameObject = gameObject;
        this.audioSource = audioSource;
        this.bounceClip = bounceClip;
        this.gameController = controller;
        paddle = paddleOBJ;
    }

    public void OnUpdate()
    {
        GameObject.transform.position += velocity * Time.deltaTime;

        Vector3 pos = GameObject.transform.position;

        if (pos.x <= -32f)
        {
            pos.x = -32f;
            velocity.x = -velocity.x;
        }
        if (pos.x >= 32f)
        {
            pos.x = 32f;
            velocity.x = -velocity.x;
        }
        if (pos.y >= 34f)
        {
            pos.y = 34f;
            velocity.y = -velocity.y;
        }
        if (pos.y <= -8f)
        {
            gameController.ReturnExtraBallToPool(this);
            return;
        }

        GameObject.transform.position = pos;

        if (IsCollidingWith(paddle))
        {
            velocity.y = Mathf.Abs(velocity.y);
            float hitPoint = (GameObject.transform.position.x - paddle.transform.position.x) / 1.5f;
            velocity.x = hitPoint * speed;

            if (bounceClip != null && audioSource != null)
                audioSource.PlayOneShot(bounceClip);
        }

        CheckCollisions();
    }

    public void LaunchBall()
    {
        float angle = Random.Range(-45f, 45f);
        float radians = angle * Mathf.Deg2Rad;
        Vector3 direction = new Vector3(Mathf.Sin(radians), Mathf.Cos(radians), 0f).normalized;
        velocity = direction * speed;
    }

    private bool IsCollidingWith(GameObject other)
    {
        if (other == null) return false;

        Vector3 ballPos = GameObject.transform.position;
        Vector3 ballSize = GameObject.transform.localScale / 2f;
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
