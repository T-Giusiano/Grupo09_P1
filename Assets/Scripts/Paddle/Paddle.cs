using UnityEngine;

public class Paddle : IUpdatable
{
    private GameObject paddleObject;
    private float speed = 20f;

    public Paddle(GameObject instance, Color color)
    {
        this.paddleObject = instance;

        // Aplica el color al crear la paleta
        ColorUtils.SetColor(paddleObject, color);

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

    public static class ColorUtils
    {
        public static void SetColor(GameObject obj, Color color)
        {
            MaterialPropertyBlock block = new MaterialPropertyBlock();
            Renderer renderer = obj.GetComponent<Renderer>();

            if (renderer != null)
            {
                block.SetColor("_Color", color);
                renderer.SetPropertyBlock(block);
            }
        }
    }
}
