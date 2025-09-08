using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public enum Weapons
{
    Pistol, Shotgun,
    Carbine, Rifle,
    Barrels, Grenades,
    C4, Rockets
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
    private List<WeaponBase> availableWeapons = new List<WeaponBase>();
    public bool canAttack;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        UnlockWeapon(0);
        foreach (WeaponBase w in weapons)
            w.gameObject.SetActive(false);
        ChangeWeapon(0);
        canAttack = true;
    }

    private void Update()
    {
        if (shooting && Time.time > nextFireTime && canAttack &&((int)currentWeapon < 4 || currentWeapon == Weapons.Rockets))
        {
            Shot();
        }
        DrawRays();
    }


    public void OnChangeWeapon(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        string keyPressed = ctx.control.name;

        if (int.TryParse(keyPressed, out int weaponIndex))
        {
            ChangeWeapon(weaponIndex - 1);
        }       
    }

    public void UnlockWeapon(int newWeapon)
    {
        availableWeapons.Add(weapons[newWeapon]);
        Debug.Log("Arma desbloqueada: " + weaponData.weapons[newWeapon].WeaponType.ToString());
        if (weapons.Length > newWeapon + 1)
            GameManager.instance.nextWeaponCost = weaponData.weapons[newWeapon + 1].cost;
    }

    public int GetLatestUnlockedWeapon()
    {
        return availableWeapons.Count - 1;
    }

    private void ChangeWeapon(int nextWeapon)
    {
        if (availableWeapons.Count < nextWeapon + 1)
        {
            Debug.Log("Arma no disponible");
            return;
        }
            
        currentWeapon = (Weapons)nextWeapon;
        foreach(WeaponBase w in availableWeapons)
        {
            w.gameObject.SetActive(false);
        }
        availableWeapons[nextWeapon].gameObject.SetActive(true);
        currentFireRate = weaponData.weapons[nextWeapon].fireRate;
        playerController.ChangeGripPoints(availableWeapons[nextWeapon].rightGripPoint, availableWeapons[nextWeapon].leftGripPoint, 
            weaponData.weapons[nextWeapon].useLeftArmBackPosition, weaponData.weapons[nextWeapon].requiresAiming, weaponData.weapons[nextWeapon].aimingOffset);
    }

    public void OnAttack(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            shooting = true;
            if (currentWeapon == Weapons.Grenades && availableWeapons[(int)currentWeapon] is GranadeWeapon granade)
            {
                granade.ResetTimer();
            }
            else if (currentWeapon == Weapons.Barrels)
            {
                if (availableWeapons[(int)currentWeapon].DeployExplosive())
                    nextFireTime = Time.time + currentFireRate;
                shooting = false;
            }
            else if (currentWeapon == Weapons.C4 && availableWeapons[(int)currentWeapon] is C4Weapon c4Weapon)
            {
                c4Weapon.HandleC4Action();            
            }
            
        }        
        else if (ctx.canceled)
        {
            if (currentWeapon == Weapons.Grenades && Time.time > nextFireTime)
            {               
                availableWeapons[(int)currentWeapon].Fire(playerController.GetLinearVelocity());
                nextFireTime = Time.time + currentFireRate;
            }
                
            shooting = false;
        }     
    }

    private void Shot()
    {
        animator.SetTrigger("Shot");
        animator.SetInteger("WeaponType", (int)currentWeapon);
        AudioManager.instance.PlayPlayerShotSound((int)currentWeapon);
        availableWeapons[(int)currentWeapon].Fire();
        nextFireTime = Time.time + currentFireRate;
    }

    public void OnTakeDamage(float duration)
    {
        canAttack = false;
        Invoke("ResetCanAttack", duration);
    }

    private void ResetCanAttack()
    {
        canAttack = true;
    }

    private void DrawRays()
    {
        Debug.DrawRay(availableWeapons[(int)currentWeapon].firePoint.position, -transform.up * 3.3f);
        Debug.DrawRay(transform.position + transform.right * 0.5f * transform.localScale.x, transform.right * transform.localScale.x);
    }
}
