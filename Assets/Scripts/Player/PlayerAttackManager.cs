using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public enum Weapons
{
    Pistol, Shotgun,
    Carbine, Rifle,
    Barrels, Grenades,
    Landmine, ChargePack, Rockets
}

public class PlayerAttackManager : MonoBehaviour
{
    [SerializeField] private Weapons currentWeapon;
    [SerializeField] private Weapon[] weapons;
    [SerializeField] private WeaponData weaponData;
    [SerializeField] private Animator animator;
    private float currentFireRate;
    private float nextFireTime;
    private PlayerController playerController;
    private bool shooting;
    private float grenadeTimer;
    public float grenadeForce;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        ChangeWeapon(0);        
    }

    private void Update()
    {
        if (shooting && Time.time > nextFireTime && ((int)currentWeapon < 4 || currentWeapon == Weapons.Rockets))
        {
            Shot();
        }
        else if (shooting && currentWeapon == Weapons.Grenades && weapons[(int)currentWeapon] is GranadeWeapon granade)
        {
            grenadeForce = Mathf.Lerp(granade.grenadeMinForce, granade.grenadeMaxForce, grenadeTimer / granade.grenadeMaxLaunchTime);
            if (grenadeTimer < granade.grenadeMaxLaunchTime) grenadeTimer += Time.deltaTime;
            else grenadeTimer = granade.grenadeMaxLaunchTime;
        }
        DrawRays();
    }


    public void OnChangeWeapon(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        string keyPressed = ctx.control.name;

        switch (keyPressed)
        {
            case "1":
                ChangeWeapon(0);
                break;
            case "2":
                ChangeWeapon(1);
                break;
            case "3":
                ChangeWeapon(2);
                break;
            case "4":
                ChangeWeapon(3);
                break;
            case "5":
                ChangeWeapon(4);
                break;
            case "6":
                ChangeWeapon(5);
                break;
            case "7":
                ChangeWeapon(6);
                break;
            case "8":
                ChangeWeapon(7);
                break;
            case "9":
                ChangeWeapon(8);
                break;
        }
    }


    private void ChangeWeapon(int nextWeapon)
    {
        currentWeapon = (Weapons)nextWeapon;
        foreach(Weapon w in weapons)
        {
            w.gameObject.SetActive(false);
        }
        weapons[nextWeapon].gameObject.SetActive(true);
        currentFireRate = weaponData.weapons[nextWeapon].fireRate;
        playerController.ChangeGripPoints(weapons[nextWeapon].rightGripPoint, weapons[nextWeapon].leftGripPoint, 
            weaponData.weapons[nextWeapon].useLeftArmBackPosition, weaponData.weapons[nextWeapon].requiresAiming, weaponData.weapons[nextWeapon].aimingOffset);
        shooting = false;
    }

    public void OnAttack(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            shooting = true;
            if (currentWeapon == Weapons.Grenades)
            {
                grenadeTimer = 0;
            }
            else if (currentWeapon == Weapons.Barrels || currentWeapon == Weapons.Landmine || currentWeapon == Weapons.ChargePack)
            {
                DeployWeapon();
                shooting = false;
            }
            
        }        
        else if (ctx.canceled)
        {
            if (currentWeapon == Weapons.Grenades && Time.time > nextFireTime)
            {               
                weapons[(int)currentWeapon].Fire(grenadeForce, playerController.GetLinearVelocity());
                nextFireTime = Time.time + currentFireRate;
                grenadeForce = 0;
                grenadeTimer = 0;
            }
                
            shooting = false;
        }     
    }

    private void Shot()
    {
        animator.SetTrigger("Shot");
        animator.SetInteger("WeaponType", (int)currentWeapon);
        weapons[(int)currentWeapon].Fire();
        nextFireTime = Time.time + currentFireRate;
    }

    private void DeployWeapon()
    {
        RaycastHit2D hit = Physics2D.Raycast(weapons[(int)currentWeapon].firePoint.position, -transform.up, 3.3f);
        if (hit.collider == null) return;
        if (!Physics2D.Raycast(transform.position + transform.right * 0.5f * transform.localScale.x, transform.right * transform.localScale.x, 1) && hit.transform.CompareTag("Ground"))
        {
            weapons[(int)currentWeapon].Deploy(hit);
            nextFireTime = Time.time + currentFireRate;
        }
    }

    private void DrawRays()
    {
        Debug.DrawRay(weapons[(int)currentWeapon].firePoint.position, -transform.up * 3.3f);
        Debug.DrawRay(transform.position + transform.right * 0.5f * transform.localScale.x, transform.right * transform.localScale.x);
    }
}
