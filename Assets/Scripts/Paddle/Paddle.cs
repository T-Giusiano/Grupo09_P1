using UnityEngine;

public class Paddle : IUpdatable
{
    private GameObject paddleObject;
    private float speed = 20f;

    public Paddle(GameObject instance)
    {
        this.paddleObject = instance;

        // Se registra para el update
        CustomUpdateManager.Instance.RegisterUpdatable(this);
    }

    public void OnUpdate()
    {
        float move = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        Vector3 newPosition = paddleObject.transform.position + new Vector3(move, 0f, 0f);

        float minX = -32f;
        float maxX = 32f;

        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        paddleObject.transform.position = newPosition;
    }

    public void Destroy()
    {
        CustomUpdateManager.Instance.UnregisterUpdatable(this);
    }
}
