using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PUPManager
{
    private static PUPManager _instance;
    public static PUPManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new PUPManager();
            return _instance;
        }
    }

    private List<PowerUpCFIG> powerUpConfigs;
    private Dictionary<string, int> spawnedCount = new Dictionary<string, int>();

    public void Initialize(List<PowerUpCFIG> configs)
    {
        if (configs == null)
        {
            Debug.LogError("No se cargaron configs de powerups en GameController.");
            return;
        }

        powerUpConfigs = configs;
        spawnedCount.Clear();

        foreach (var config in powerUpConfigs)
            spawnedCount[config.powerUpName] = 0;
    }

    public bool CanSpawnPowerUp(string powerUpName)
    {
        PowerUpCFIG config = GetConfig(powerUpName);
        if (config == null) return false;

        return spawnedCount[powerUpName] < config.maxInstancesPerLevel;
    }

    public void RegisterPowerUp(string powerUpName)
    {
        if (spawnedCount.ContainsKey(powerUpName))
            spawnedCount[powerUpName]++;
    }

    public PowerUpCFIG GetConfig(string powerUpName)
    {
        return powerUpConfigs.Find(p => p.powerUpName == powerUpName);
    }
}
