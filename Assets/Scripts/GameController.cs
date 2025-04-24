using UnityEngine;

public class GameController : MonoBehaviour
{
    void Update()
    {
        CustomUpdateManager.Instance.CustomUpdate();
    }
}
