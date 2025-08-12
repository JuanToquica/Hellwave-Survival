using UnityEngine;

public enum Weapons
{
    Pistol, Carbine,
    Rifle, Shotgun,
    Barrels, Grenades,
    FakeWalls, Landmine,
    Rockets, ChargePack
}

public class PlayerAttackManager : MonoBehaviour
{
    public Weapons currentWeapon;
    public float currentCooldown;
    void Start()
    {
        
    }

}
