using UnityEngine;

public class PaddleManager : MonoBehaviour, IUpdatable
{
    private Paddle paddle;

    [SerializeField] private Color paddleColor = Color.white;

    [Header("Parallax Settings")]
    [SerializeField] private string[] parallaxAddresses;
    [SerializeField] private float[] parallaxFactors;
    [SerializeField] private Transform parallaxParent;

    private ParallaxSystem parallaxSystem;

    private void OnEnable()
    {
        paddle = new Paddle(this.gameObject, paddleColor);
        CustomUpdateManager.Instance.RegisterUpdatable(this);

        parallaxSystem = new ParallaxSystem(transform);

        for (int i = 0; i < parallaxAddresses.Length && i < parallaxFactors.Length; i++)
        {
            int index = i;
            ParallaxLoader.LoadLayer(parallaxAddresses[i], parallaxParent, loadedTransform =>
            {
                parallaxSystem.AddLayer(loadedTransform, parallaxFactors[index]);
            });
        }

        CustomUpdateManager.Instance.RegisterUpdatable(parallaxSystem);
    }

    private void OnDisable()
    {
        if (CustomUpdateManager.Instance != null)
        {
            CustomUpdateManager.Instance.UnregisterUpdatable(this);
            CustomUpdateManager.Instance.UnregisterUpdatable(parallaxSystem);
        }          
    }

    public void OnUpdate()
    {
        paddle.OnUpdate();
    }
}
