using System;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public static event Action<WeaponBase> OnWeaponChanged;
    public static event Action<string> OnWeaponUnlocked;
    public static event Action<string> OnCollectedAmmo;

    [SerializeField] private WeaponBase[] weapons;
    [SerializeField] private WeaponData weaponData;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject weaponsParent;
    private float currentFireRate;
    private float nextFireTime;
    private PlayerController playerController;
    private PlayerHealth playerHealth;
    public bool shooting;
    private List<WeaponBase> availableWeapons;
    private bool canAttack;
    private int nextWeaponCost;
    private int currentWeaponIndex;
    private Vector2 scrollInput;


    private Weapons _currentWeapon;
    public Weapons currentWeapon
    {
        get { return _currentWeapon; }
        set
        {
            if (_currentWeapon != value)
            {
                _currentWeapon = value;
                OnWeaponChanged?.Invoke(availableWeapons[(int)_currentWeapon]);
            }
        }
    }

    private void OnEnable()
    {
        GameManager.OnEnemiesKilledChanged += CheckForWeaponUnlock;
        PlayerHealth.OnPlayerDeath += OnDie;
    }
       
    private void OnDisable()
    {
        GameManager.OnEnemiesKilledChanged -= CheckForWeaponUnlock;
        PlayerHealth.OnPlayerDeath -= OnDie;
    }

    private void Awake()
    {
        availableWeapons = new List<WeaponBase>();
    }

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        playerHealth = GetComponent<PlayerHealth>();      
        UnlockWeapon(0);
        OnWeaponChanged?.Invoke(availableWeapons[(int)_currentWeapon]);
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
    
    public void OnScrollWeapon(InputAction.CallbackContext context)
    {
        scrollInput = context.ReadValue<Vector2>();

        if (context.performed)
        {
            if (scrollInput.y > 0)
            {
                currentWeaponIndex++;

                for (int i = 0; i <= availableWeapons.Count; i ++)
                {
                    if (currentWeaponIndex >= availableWeapons.Count) currentWeaponIndex = 0;
                    if (availableWeapons[currentWeaponIndex].Ammo == 0)
                    {
                        currentWeaponIndex++;
                        continue;
                    }                        
                    break;
                }
                ChangeWeapon(currentWeaponIndex);
            }
            else if (scrollInput.y < 0)
            {
                currentWeaponIndex--;

                for (int i = 0; i <= availableWeapons.Count; i++)
                {
                    if (currentWeaponIndex < 0) currentWeaponIndex = availableWeapons.Count -1;
                    if (availableWeapons[currentWeaponIndex].Ammo == 0)
                    {
                        currentWeaponIndex--;
                        continue;
                    }
                    break;
                }
                ChangeWeapon(currentWeaponIndex);
            }
        }
    }

    private void CheckForWeaponUnlock(int deadEnemies)
    {
        if (availableWeapons == null) return;
        if (deadEnemies == nextWeaponCost)
            UnlockWeapon(availableWeapons.Count);
    }

    public void UnlockWeapon(int newWeapon)
    {
        if (newWeapon < availableWeapons.Count) return;
        availableWeapons.Add(weapons[newWeapon]);
        if (newWeapon != 0)
            OnWeaponUnlocked?.Invoke(weaponData.weapons[newWeapon].WeaponType.ToString() + " Unlocked");
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
            AudioManager.instance.PlayDryFireSound();
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
        currentWeaponIndex = nextWeapon;
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
        int random = UnityEngine.Random.Range(0, availableWeapons.Count);
        if (random == 0)
        {
            playerHealth.Heal(); //Curar player, ya que pistol tiene municion infinita
            return;
        }      
        for (int i = 0; i < 5; i++) //Escoger arma a recargar 5 veces para mas probabilidad de que sea un arma descargada
        {
            if (availableWeapons[random].Ammo == weaponData.weapons[random].maxAmmo)
            {
                random = UnityEngine.Random.Range(1, availableWeapons.Count);
                continue;
            }              
            availableWeapons[random].TakeAmmunition();
            Debug.Log("Municion recogida para: " + availableWeapons[random].name);
            OnCollectedAmmo?.Invoke(weaponData.weapons[random].WeaponType.ToString() + " Ammo Collected");
            return;
        }
        availableWeapons[random].TakeAmmunition();
        Debug.Log("Municion recogida para: " + availableWeapons[random].name);
        OnCollectedAmmo?.Invoke(weaponData.weapons[random].WeaponType.ToString() + " Ammo Collected");
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

    public void OnDie()
    {
        shooting = false;
        weaponsParent.SetActive(false);
        this.enabled = false;
    }
}
