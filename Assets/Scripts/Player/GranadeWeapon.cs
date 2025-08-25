using UnityEditor.Profiling.Memory.Experimental;
using UnityEngine;

public class GranadeWeapon : Weapon
{
    [HideInInspector] public int grenadeMaxForce;
    [HideInInspector] public int grenadeMinForce;
    [HideInInspector] public float grenadeMaxLaunchTime;
    [SerializeField] private GrenadeData grenadeData;

    protected override void OnEnable()
    {
        base.OnEnable();
        this.grenadeMaxForce = grenadeData.grenadeMaxForce;
        this.grenadeMinForce = grenadeData.grenadeMinForce;
        this.grenadeMaxLaunchTime = grenadeData.grenadeMaxLaunchTime;      
    }

    public override void Fire(float force)
    {
        GranadeController granade = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation).GetComponent<GranadeController>();
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - firePoint.position).normalized;
        granade.Launch(direction, force);
    }
}
