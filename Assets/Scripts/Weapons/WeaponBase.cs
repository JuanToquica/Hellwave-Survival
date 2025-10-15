using System;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    public event Action<int, int> OnAmmoChanged;
    public GameObject weaponGameobject;
    public Transform firePoint;
    [SerializeField] protected Transform player;    
    [SerializeField] protected WeaponData weaponData;
    [SerializeField] protected PlayerAttackManager playerAttackManager;
    public int weaponIndex;
    public Sprite ammoIcon;
    public Transform rightGripPoint;
    public Transform leftGripPoint;
    protected int damage;
    protected float knockBackForce;
    public int MaxAmmo;
    public int _Ammo;
    public int Ammo
    {
        get { return _Ammo; }
        protected set
        {
            if (_Ammo != value)
            {
                _Ammo = value;
                OnAmmoChanged?.Invoke(_Ammo, MaxAmmo);
            }
        }
    }

    protected void Awake()
    {
        Ammo = weaponData.weapons[weaponIndex].maxAmmo;
        MaxAmmo = weaponData.weapons[weaponIndex].maxAmmo;
    }

    protected virtual void OnEnable() 
    {
        MaxAmmo = weaponData.weapons[weaponIndex].maxAmmo;
        damage = weaponData.weapons[weaponIndex].damage;
        knockBackForce = weaponData.weapons[weaponIndex].knockBackForce;
        OnAmmoChanged?.Invoke(_Ammo, MaxAmmo);
    }

    public virtual void Fire() { }
    public virtual void Fire(Vector2 playerVelocity) { }
    public virtual bool DeployExplosive() { return true; }
    
    public void SetAmmo()
    {
        MaxAmmo = weaponData.weapons[weaponIndex].maxAmmo;
        Ammo = weaponData.weapons[weaponIndex].maxAmmo;
    }

    public void TakeAmmunition()
    {
        Ammo += weaponData.weapons[weaponIndex].magazineCapacity;
        if (Ammo > MaxAmmo)
            Ammo = MaxAmmo;
    }
}
