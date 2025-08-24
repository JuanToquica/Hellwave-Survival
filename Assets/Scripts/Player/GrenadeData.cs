using UnityEngine;

[CreateAssetMenu(fileName = "GrenadeData", menuName = "ScriptableObject/GrenadeData")]
public class GrenadeInfo : ScriptableObject
{
    public int grenadeMaxForce;
    public int grenadeMinForce;
    public float grenadeMaxLaunchTime;
}
