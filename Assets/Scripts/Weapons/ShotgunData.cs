using UnityEngine;

[CreateAssetMenu(fileName = "ShotgunData", menuName = "ScriptableObject/ShotgunData")]
public class ShotgunData : ScriptableObject
{
    public int spreadAngle;
    public int amountOfBullets;
    public int range;
}

