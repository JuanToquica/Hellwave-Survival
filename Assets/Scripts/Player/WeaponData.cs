using UnityEngine;

[System.Serializable]
public class WeaponInfo
{
    public int cost;
    public int damage;
    public int cooldown;
    public int magazineCapacity;
    public int maxAmmo;
}


[CreateAssetMenu(fileName = "WeaponData", menuName = "ScriptableObject/WeaponsData")]
public class WeaponData : ScriptableObject
{
    public WeaponInfo Pistol;
    public WeaponInfo Shotgun;
    public WeaponInfo Carbine;
    public WeaponInfo AssaultRifle;
    public WeaponInfo Barrels;
    public WeaponInfo Grenades;
    public WeaponInfo FakeWalls;
    public WeaponInfo landmine;
    public WeaponInfo Rockets;
    public WeaponInfo ChargePack;

}
