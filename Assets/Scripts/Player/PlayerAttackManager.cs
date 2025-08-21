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
    public PlayerController playerController;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        ChangeWeapon(0);      
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

        playerController.ChangeGripPoints(weapons[nextWeapon].rightGripPoint, weapons[nextWeapon].leftGripPoint, 
            weaponData.weapons[nextWeapon].useLeftArmBackPosition, weaponData.weapons[nextWeapon].requiresAiming);
    }

}
