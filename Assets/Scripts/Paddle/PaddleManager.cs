using UnityEngine;

public class PaddleManager :  IUpdatable
{
    private Paddle paddle;
    private ParallaxSystem parallaxSystem;
    private GameObject paddleObject;

    public PaddleManager(GameObject paddleObject, string[] parallaxAddresses, float[] parallaxFactors, Transform parallaxParent)
    {
        this.paddleObject = paddleObject;

        paddle = new Paddle(paddleObject);
        CustomUpdateManager.Instance.RegisterUpdatable(this);

        parallaxSystem = new ParallaxSystem(paddleObject.transform);

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

    public void OnUpdate()
    {
        paddle.OnUpdate();
    }

    public void Destroy()
    {
        CustomUpdateManager.Instance.UnregisterUpdatable(this);
        CustomUpdateManager.Instance.UnregisterUpdatable(parallaxSystem);
    }
}
