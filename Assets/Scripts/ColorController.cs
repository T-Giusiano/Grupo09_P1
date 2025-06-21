using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorController
{
    private static MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();

    public static void SetColorByTag(GameObject obj)
    {
        if (obj.TryGetComponent<Renderer>(out Renderer renderer))
        {
            Color colorToApply = Color.white;

            switch (obj.tag)
            {
                case "Paddle":
                    colorToApply = Color.blue;
                    break;
                case "Ball":
                    colorToApply = Color.red;
                    break;
                case "ExtraBall":
                    colorToApply = Color.green;
                    break;
                case "PowerUp":
                    colorToApply = Color.green;
                    break;
                case "Margin":
                    colorToApply = Color.black;
                    break;
                case "LoseMargin":
                    colorToApply = Color.black;
                    break;
                default:
                    colorToApply = Color.white;
                    break;
            }

            propertyBlock.SetColor("_Color", colorToApply);
            renderer.SetPropertyBlock(propertyBlock);
        }
    }
}
