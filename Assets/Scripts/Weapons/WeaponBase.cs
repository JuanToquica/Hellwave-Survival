using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    public Transform firePoint;
    [SerializeField] protected Transform player;    
    [SerializeField] protected WeaponData weaponData;  
    [SerializeField] protected int weaponIndex;
    public Transform rightGripPoint;
    public Transform leftGripPoint;
    protected int damage;

    protected virtual void OnEnable() { }
    public virtual void Fire() { }
    public virtual void Fire(Vector2 playerVelocity) { }
    public virtual bool DeployExplosive() { return true; }
    
}
