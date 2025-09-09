using UnityEditor.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.WSA;

public class GranadeWeapon : ExplosiveWeapon
{
    [HideInInspector] private int grenadeMaxForce;
    [HideInInspector] private int grenadeMinForce;
    [HideInInspector] private float grenadeMaxLaunchTime;
    [SerializeField] private GrenadeData grenadeData;
    [SerializeField] private PlayerAttackManager attackManager;
    private float force;
    private float timer;

    protected override void OnEnable()
    {
        base.OnEnable();
        this.grenadeMaxForce = grenadeData.grenadeMaxForce;
        this.grenadeMinForce = grenadeData.grenadeMinForce;
        this.grenadeMaxLaunchTime = grenadeData.grenadeMaxLaunchTime;      
    }

    private void Update()
    {
        if (attackManager.shooting)
        {
            force = Mathf.Lerp(grenadeMinForce, grenadeMaxForce, timer / grenadeMaxLaunchTime);
            if (timer < grenadeMaxLaunchTime) timer += Time.deltaTime;
            else timer = grenadeMaxLaunchTime;
        }
    }

    public void ResetTimer()
    {
        timer = 0f;
    }

    public override void Fire(Vector2 playerVelocity)
    {
        timer = 0;
        GrenadeController granade = Instantiate(explosivePrefab, firePoint.position, firePoint.rotation).GetComponent<GrenadeController>();
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - firePoint.position).normalized;
        granade.Initialize(explosionRadius, explosionDelay, damage);
        granade.Launch(direction, force, playerVelocity);
        force = 0;
        Ammo--;
        if (Ammo == 0)
        {
            playerAttackManager.OnOutOfAmmunition();
        }
    }
}
