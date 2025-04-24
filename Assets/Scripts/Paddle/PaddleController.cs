using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleController : MonoBehaviour, IUpdatable
{
    private float speed = 20f;

    private void OnEnable()
    {
        CustomUpdateManager.Instance.RegisterUpdatable(this);
    }

    private void OnDisable()
    {
        if (CustomUpdateManager.Instance != null)
        {
            CustomUpdateManager.Instance.UnregisterUpdatable(this);
        }
    }

    public void OnUpdate()
    {
        float move = Input.GetAxis("Horizontal") * speed * Time.deltaTime;

        Vector3 newPosition = transform.position + new Vector3(move, 0f, 0f);
        transform.position = newPosition;

        float minX = -32f;
        float maxX = 32f;
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, minX, maxX), transform.position.y, transform.position.z);
    }
}
