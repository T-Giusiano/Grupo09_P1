using UnityEngine;


[CreateAssetMenu(fileName = "PowerUpConfig", menuName = "Configs/PowerUpConfig")]
public class PowerUpCFIG : ScriptableObject
{
    public string powerUpName;
    public int maxInstancesPerLevel;
    public int ballsToSpawn;
}
