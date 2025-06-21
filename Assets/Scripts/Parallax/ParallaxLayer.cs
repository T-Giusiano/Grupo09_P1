using UnityEngine;

public class ParallaxLayer
{
    private Transform layerTransform;
    private float factor;
    private Vector3 startPos;

    public ParallaxLayer(Transform transform, float parallaxFactor)
    {
        layerTransform = transform;
        factor = parallaxFactor;
        startPos = transform.position;
    }

    public void UpdatePosition(Vector3 targetPosition)
    {
        Vector3 newPos = startPos;
        newPos.x += targetPosition.x * factor;
        layerTransform.position = newPos;
    }
}
