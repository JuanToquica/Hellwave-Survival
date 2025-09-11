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
    private PlayerHealth playerHealth;
    public bool shooting;
    private List<WeaponBase> availableWeapons = new List<WeaponBase>();
    private bool canAttack;
    private int nextWeaponCost;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        playerHealth = GetComponent<PlayerHealth>();
        GameManager.OnEnemiesKilledChanged += CheckForWeaponUnlock;
        UnlockWeapon(0);
        foreach (WeaponBase w in weapons)
            w.gameObject.SetActive(false);
        ChangeWeapon(0);
        canAttack = true;
    }

    private void Update()
    {
        if (shooting && Time.time > nextFireTime && canAttack &&((int)currentWeapon < 4 || currentWeapon == Weapons.Rockets) && IsClearToShoot())
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

    private void CheckForWeaponUnlock(int deadEnemies)
    {
        if (deadEnemies == nextWeaponCost)
            UnlockWeapon(availableWeapons.Count);
    }

    public void UnlockWeapon(int newWeapon)
    {
        availableWeapons.Add(weapons[newWeapon]);
        Debug.Log("Arma desbloqueada: " + weaponData.weapons[newWeapon].WeaponType.ToString());
        availableWeapons[newWeapon].SetAmmo();
        if (weapons.Length > newWeapon + 1)
            nextWeaponCost = weaponData.weapons[newWeapon + 1].cost;
        else
            GameManager.OnEnemiesKilledChanged -= CheckForWeaponUnlock;
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
        bool canChange = true; 
        if (availableWeapons[nextWeapon].Ammo == 0)
        {
            canChange = false;
            if (nextWeapon == 6 && availableWeapons[nextWeapon] is C4Weapon c4) //Excepcion para los c4, puede haber algun sin explotar
            {
                if (c4.c4sPlaced) canChange = true;
            }           
        }
        if (!canChange)
        {
            Debug.Log("Arma sin Municion");
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

            if (!IsClearToShoot()) return;

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
            if (currentWeapon == Weapons.Grenades && Time.time > nextFireTime && IsClearToShoot())
            {               
                availableWeapons[(int)currentWeapon].Fire(playerController.GetLinearVelocity());
                nextFireTime = Time.time + currentFireRate;
            }             
            shooting = false;
        }     
    }

    private bool IsClearToShoot()
    {
        Vector3 direction = availableWeapons[(int)currentWeapon].firePoint.position - transform.position;
        float distance = Vector3.Distance(transform.position, availableWeapons[(int)currentWeapon].firePoint.position) + 0.02f;

        return !Physics2D.Raycast(transform.position, direction, distance, 1 << 3); //Verifica que el arma no este atravesando alguna pared
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

    public void CollectAmmunition()
    {
        int random = Random.Range(0, availableWeapons.Count);
        if (random == 0)
        {
            playerHealth.Heal(); //Curar player, ya que pistol tiene municion infinita
            return;
        }      
        for (int i = 0; i < 3; i++) //Escoger arma a recargar 3 veces para mas probabilidad de que sea un arma descargada
        {
            if (availableWeapons[random].Ammo == weaponData.weapons[random].maxAmmo)
            {
                random = Random.Range(1, availableWeapons.Count);
                continue;
            }              
            availableWeapons[random].TakeAmmunition();
            Debug.Log("Municion recogida para: " + availableWeapons[random].name);
            return;
        }
        availableWeapons[random].TakeAmmunition();
        Debug.Log("Municion recogida para: " + availableWeapons[random].name);
    }


    public void OnOutOfAmmunition()
    {
        ChangeWeapon(0);
    }

    private void DrawRays()
    {
        Debug.DrawRay(availableWeapons[(int)currentWeapon].firePoint.position, -transform.up * 3.3f);
        Debug.DrawRay(transform.position + transform.right * 0.5f * transform.localScale.x, transform.right * transform.localScale.x);

        Vector3 direction = (availableWeapons[(int)currentWeapon].firePoint.position - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, availableWeapons[(int)currentWeapon].firePoint.position) + 0.02f;
        Debug.DrawRay(transform.position, direction * distance, Color.red);
    }
}
