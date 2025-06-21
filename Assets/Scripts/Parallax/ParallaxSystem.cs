using System.Collections.Generic;
using UnityEngine;

public class ParallaxSystem : IUpdatable
{
    private List<ParallaxLayer> layers = new List<ParallaxLayer>();
    private Transform targetTransform;

    public ParallaxSystem(Transform target)
    {
        targetTransform = target;
    }

    public void AddLayer(Transform layer, float factor)
    {
        layers.Add(new ParallaxLayer(layer, factor));
    }

    public void OnUpdate()
    {
        foreach (var layer in layers)
        {
            layer.UpdatePosition(targetTransform.position);
        }
    }
}
