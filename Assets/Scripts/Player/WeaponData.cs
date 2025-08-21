using UnityEngine;

[System.Serializable]
public class WeaponInfo
{
    public Weapons WeaponType;
    public int cost;
    public int damage;
    public int cooldown;
    public int magazineCapacity;
    public int maxAmmo;
    public bool useLeftArmBackPosition;
    public bool requiresAiming;
}


[CreateAssetMenu(fileName = "WeaponData", menuName = "ScriptableObject/WeaponsData")]
public class WeaponData : ScriptableObject
{
    public WeaponInfo[] weapons;
}
