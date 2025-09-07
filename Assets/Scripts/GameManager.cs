using UnityEngine;
using UnityEngine.InputSystem.HID;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("UI")]
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private GameObject gameOverPanel;

    [Header("Gameplay")]
    public bool isTheGamePaused;
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

    public void PauseAndUnpauseGame()
    {
        if (isTheGamePaused)
        {
            Time.timeScale = 1;
            pauseUI.SetActive(false);
            isTheGamePaused = false;
            //Cursor.lockState = CursorLockMode.Locked;
            //Cursor.visible = false;
        }
        else
        {
            Time.timeScale = 0;
            pauseUI.SetActive(true);
            isTheGamePaused = true;
            //Cursor.lockState = CursorLockMode.None;
            //Cursor.visible = true;
        }
    }
}
