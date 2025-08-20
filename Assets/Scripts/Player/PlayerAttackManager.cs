using UnityEngine;
using UnityEngine.InputSystem;

public enum Weapons
{
    Pistol, Carbine,
    Rifle, Shotgun,
    Barrels, Grenades,
    FakeWalls, Landmine,
    Rockets, ChargePack
}

public class PlayerAttackManager : MonoBehaviour
{
    [SerializeField] private Weapons currentWeapon;
    private float currentCooldown;
    [SerializeField] private GameObject[] weapons;



    void Start()
    {
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
        }
    }


    private void ChangeWeapon(int weapon)
    {
        currentWeapon = (Weapons)weapon;
        foreach(GameObject w in weapons)
        {
            w.SetActive(false);
        }
        weapons[weapon].SetActive(true);
    }


}
