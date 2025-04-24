using UnityEngine;

public class PowerUp : MonoBehaviour, IUpdatable
{
    public float fallSpeed = 3f;

    void OnEnable()
    {
        CustomUpdateManager.Instance.RegisterUpdatable(this);
    }

    void OnDisable()
    {
        CustomUpdateManager.Instance.UnregisterUpdatable(this);
    }

    public void OnUpdate()
    {
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;

        if (transform.position.y <= -8f)
        {
            Destroy(gameObject);
        }

        GameObject paddle = GameObject.Find("Paddle");
        if (IsCollidingWith(paddle))
        {
            Vector3 spawnPos = new Vector3(paddle.transform.position.x, paddle.transform.position.y + 0.5f, 0f);
            GameObject extraBall = Resources.Load<GameObject>("Prefabs/ExtraBall");
            Instantiate(extraBall, spawnPos, Quaternion.identity);
            Instantiate(extraBall, spawnPos, Quaternion.identity);

            Destroy(gameObject);
        }
    }

    bool IsCollidingWith(GameObject other)
    {
        Vector3 thisPos = transform.position;
        Vector3 thisSize = transform.localScale / 2f;
        Vector3 otherPos = other.transform.position;
        Vector3 otherSize = other.transform.localScale / 2f;

        return (Mathf.Abs(thisPos.x - otherPos.x) < (thisSize.x + otherSize.x)) &&
               (Mathf.Abs(thisPos.y - otherPos.y) < (thisSize.y + otherSize.y));
    }
}
