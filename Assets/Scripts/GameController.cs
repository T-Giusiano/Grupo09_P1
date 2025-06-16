using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private List<PowerUpCFIG> configs;

    private void Start()
    {
        PUPManager.Instance.Initialize(configs);
    }

    private void Update()
    {
        CustomUpdateManager.Instance.CustomUpdate();
    }
}
