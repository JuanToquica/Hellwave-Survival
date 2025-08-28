using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public enum Weapons
{
    Pistol, Shotgun,
    Carbine, Rifle,
    Barrels, Grenades,
    Landmine, C4, Rockets
}

public class PlayerAttackManager : MonoBehaviour
{
    [SerializeField] private Weapons currentWeapon;
    [SerializeField] private WeaponBase[] weapons;
    [SerializeField] private WeaponData weaponData;
    [SerializeField] private Animator animator;
    private float currentFireRate;
    private float nextFireTime;
    private PlayerController playerController;
    public bool shooting;
    //private List<WeaponBase> weapons = new List<WeaponBase>();

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        ChangeWeapon(0);
        //weapons.Add(weapons[0]);
    }

    private void Update()
    {
        if (shooting && Time.time > nextFireTime && ((int)currentWeapon < 4 || currentWeapon == Weapons.Rockets))
        {
            Shot();
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
        foreach(WeaponBase w in weapons)
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
            if (currentWeapon == Weapons.Grenades && weapons[(int)currentWeapon] is GranadeWeapon granade)
            {
                granade.ResetTimer();
            }
            else if (currentWeapon == Weapons.Barrels || currentWeapon == Weapons.Landmine)
            {
                if (weapons[(int)currentWeapon].DeployExplosive())
                    nextFireTime = Time.time + currentFireRate;
                shooting = false;
            }
            else if (currentWeapon == Weapons.C4 && weapons[(int)currentWeapon] is C4Weapon c4Weapon)
            {
                c4Weapon.HandleC4Action();            
            }
            
        }        
        else if (ctx.canceled)
        {
            if (currentWeapon == Weapons.Grenades && Time.time > nextFireTime)
            {               
                weapons[(int)currentWeapon].Fire(playerController.GetLinearVelocity());
                nextFireTime = Time.time + currentFireRate;
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

    private void DrawRays()
    {
        Debug.DrawRay(weapons[(int)currentWeapon].firePoint.position, -transform.up * 3.3f);
        Debug.DrawRay(transform.position + transform.right * 0.5f * transform.localScale.x, transform.right * transform.localScale.x);
    }
}
