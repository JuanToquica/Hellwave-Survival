using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int deadEnemies;
    public int nextWeaponCost;
    public PlayerAttackManager playerAttack;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void OnEnemyDead()
    {
        deadEnemies++;
        if (deadEnemies == nextWeaponCost)
            playerAttack.UnlockWeapon(playerAttack.GetLatestUnlockedWeapon() + 1);
    }
}
