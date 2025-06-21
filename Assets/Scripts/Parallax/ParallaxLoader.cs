using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ParallaxLoader
{
    public static void LoadLayer(string address, Transform parent, Action<Transform> onLoaded)
    {
        Addressables.InstantiateAsync(address, parent).Completed += handle =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                Transform layerTransform = handle.Result.transform;
                onLoaded?.Invoke(layerTransform);
            }
            else
            {
                Debug.LogError($"Fail to load Addressable: {address}");
            }
        };
    }
}
