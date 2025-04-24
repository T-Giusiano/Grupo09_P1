using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorController : MonoBehaviour
{
    MaterialPropertyBlock propertyBlock;
    public Color color;

    private void Awake()
    {
        if (propertyBlock == null)
            propertyBlock = new MaterialPropertyBlock();

        Renderer renderer = GetComponent<Renderer>();

        propertyBlock.SetColor("_Color", color);

        renderer.SetPropertyBlock(propertyBlock);

    }
}
