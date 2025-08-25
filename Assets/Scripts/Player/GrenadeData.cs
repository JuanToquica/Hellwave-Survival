using UnityEngine;

[CreateAssetMenu(fileName = "GrenadeData", menuName = "ScriptableObject/GrenadeData")]
public class GrenadeData : ScriptableObject
{
    public int grenadeMaxForce;
    public int grenadeMinForce;
    public float grenadeMaxLaunchTime;
    public float fuseTime;
    public float extraMultiplier;
}
