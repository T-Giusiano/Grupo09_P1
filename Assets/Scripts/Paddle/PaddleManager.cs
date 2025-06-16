using UnityEngine;

public class PaddleManager : MonoBehaviour, IUpdatable
{
    private Paddle paddle;

    [SerializeField] private Color paddleColor = Color.white;

    private void OnEnable()
    {
        paddle = new Paddle(this.gameObject, paddleColor);
        CustomUpdateManager.Instance.RegisterUpdatable(this);
    }

    private void OnDisable()
    {
        if (CustomUpdateManager.Instance != null)
            CustomUpdateManager.Instance.UnregisterUpdatable(this);
    }

    public void OnUpdate()
    {
        paddle.OnUpdate();
    }
}
