using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    public Transform firePoint;
    [SerializeField] protected Transform player;    
    [SerializeField] protected WeaponData weaponData;
    [SerializeField] protected PlayerAttackManager playerAttackManager;
    [SerializeField] protected int weaponIndex;
    public Transform rightGripPoint;
    public Transform leftGripPoint;
    protected int damage;
    public int Ammo;

    protected virtual void OnEnable() { }
    public virtual void Fire() { }
    public virtual void Fire(Vector2 playerVelocity) { }
    public virtual bool DeployExplosive() { return true; }
    
    public void SetAmmo()
    {
        Ammo = weaponData.weapons[weaponIndex].maxAmmo;
    }

    public void TakeAmmunition()
    {
        Ammo += weaponData.weapons[weaponIndex].magazineCapacity;
        if (Ammo > weaponData.weapons[weaponIndex].maxAmmo)
            Ammo = weaponData.weapons[weaponIndex].maxAmmo;
    }
}
