using UnityEngine;

[System.Serializable]
public class WeaponInfo
{
    public Weapons WeaponType;
    public int cost;
    public int speed;
    public int range;
    public int damage;
    public float fireRate;
    public int magazineCapacity;
    public int maxAmmo;
    public bool useLeftArmBackPosition;
    public bool requiresAiming;
    [Tooltip ("offset applied to the weapon pivot when calculating the direction toward the mouse and ensuring that the weapon appears aligned")] 
    public float aimingOffset;
    public float knockBackForce;
}

[CreateAssetMenu(fileName = "WeaponData", menuName = "ScriptableObject/WeaponsData")]
public class WeaponData : ScriptableObject
{
    public WeaponInfo[] weapons;
}
