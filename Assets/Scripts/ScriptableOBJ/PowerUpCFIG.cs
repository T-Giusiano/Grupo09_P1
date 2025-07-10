using UnityEngine;


[CreateAssetMenu(fileName = "PowerUPConfig", menuName = "Configs/PowerUPConfig")]
public class PowerUpCFIG : ScriptableObject
{
    public string powerUpName;
    public int maxInstancesPerLevel;
    public int increasePU;
}
