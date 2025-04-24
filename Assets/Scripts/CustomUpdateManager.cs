using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomUpdateManager : MonoBehaviour
{
    private static CustomUpdateManager _instance;

    private List<IUpdatable> updatables = new List<IUpdatable>();
    private static bool isShuttingDown = false;

    public static CustomUpdateManager Instance
    {
        get
        {
            if (_instance == null && !isShuttingDown)
            {
                _instance = FindObjectOfType<CustomUpdateManager>();

                if (_instance == null)
                {
                    GameObject go = new GameObject("CustomUpdateManager");
                    _instance = go.AddComponent<CustomUpdateManager>();
                }
            }

            return _instance;
        }
    }

    public void RegisterUpdatable(IUpdatable updatable)
    {
        if (!updatables.Contains(updatable))
        {
            updatables.Add(updatable);
        }
    }

    public void UnregisterUpdatable(IUpdatable updatable)
    {
        if (updatables.Contains(updatable))
        {
            updatables.Remove(updatable);
        }
    }

    public void CustomUpdate()
    {
        for (int i = 0; i < updatables.Count; i++)
        {
            updatables[i].OnUpdate();
        }
    }

    public void ClearUpdatables()
    {
        updatables.Clear();
    }

    private void Update()
    {
        CustomUpdate();
    }
    private void OnApplicationQuit()
    {
        isShuttingDown = true;
    }
}
public interface IUpdatable
{
    void OnUpdate();
}

