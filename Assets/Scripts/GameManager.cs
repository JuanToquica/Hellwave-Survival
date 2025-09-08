using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("UI")]
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private GameObject gameOverPanel;

    [Header("Gameplay")]
    [SerializeField] private GameObject enemyMeele;
    [SerializeField] private GameObject enemyShooter;
    [SerializeField] private Transform player;
    [SerializeField] private Transform[] spawners;
    [SerializeField] private float timeBetweenSpawns;
    [SerializeField] private int minNumerOfSpawners;
    [SerializeField] private int maxNumerOfSpawners;    
    [SerializeField] private int numberOfEnemiesRoundOne;
    [SerializeField] private int additionOfEnemiesPerRound;
    [SerializeField] private int maxNumberOfEnemiesPerRound;
    public int activedSpawners;
    public int aliveEnemies;
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

    private void Start()
    {
        activedSpawners = minNumerOfSpawners;
        StartRound();
    }

    private void Update()
    {
        
    }

    public void OnEnemyDead()
    {
        deadEnemies++;
        if (deadEnemies == nextWeaponCost)
            playerAttack.UnlockWeapon(playerAttack.GetLatestUnlockedWeapon() + 1);
    }

    public void OnPauseClicked(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            PauseAndUnpauseGame();
        }
    }

    public void PauseAndUnpauseGame()
    {
        if (isTheGamePaused)
        {
            Time.timeScale = 1;
            pauseUI.SetActive(false);
            isTheGamePaused = false;
            InputManager.instance.EnablePlayerInputs();
            //Cursor.lockState = CursorLockMode.Locked;
            //Cursor.visible = false;
        }
        else
        {
            Time.timeScale = 0;
            pauseUI.SetActive(true);
            isTheGamePaused = true;
            InputManager.instance.DisablePlayerInputs();
            //Cursor.lockState = CursorLockMode.None;
            //Cursor.visible = true;
        }
        AudioManager.instance.PlayButtonSound();        
    }

    private void StartRound()
    {
        StartCoroutine(spawnAllEnemies(numberOfEnemiesRoundOne));
    }

    private void EndRound()
    {

    }

    [ContextMenu("Spawn round")]
    private void SpawnEnemyWave()
    {
        for (int i = 0; i < activedSpawners; i++)
        {
            GameObject enemy = Instantiate(enemyMeele, spawners[i].position, Quaternion.identity);
            EnemyMeeleAttack enemyAttack = enemy.GetComponent<EnemyMeeleAttack>();
            enemyAttack.SetPlayer(player);
            EnemyController enemyController = enemy.GetComponent<EnemyController>();
            enemyController.SetPlayer(player);
            aliveEnemies++;
        }
    }

    private IEnumerator spawnAllEnemies(int numberOfEnemies)
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            SpawnEnemyWave();
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }
}
