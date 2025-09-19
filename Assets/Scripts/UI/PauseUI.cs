using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections;
using TMPro;

public class PauseUI : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private PlayerAttackManager playerAttack;
    [SerializeField] private TextMeshProUGUI[] weaponsState;
    [SerializeField] private Color lockedWeaponTextColor;
    [SerializeField] private Color unlockedWeaponTextColor;
    [SerializeField] private GameObject settingsUI;

    private void OnEnable()
    {
        int LatestUnlockedWeapon = playerAttack.GetLatestUnlockedWeapon();
        for (int i = 0; i < weaponsState.Length; i++)
        {
            if (i <= LatestUnlockedWeapon)
            {
                weaponsState[i].text = "[Unlocked]";
                weaponsState[i].color = unlockedWeaponTextColor;
            }               
            else
            {
                weaponsState[i].text = "[Locked]";
                weaponsState[i].color = lockedWeaponTextColor;
            }               
        }
    }

    private void Start()
    {
        resumeButton.onClick.AddListener(OnResumeButtonClicked);
        exitButton.onClick.AddListener(OnExitButtonClicked);
        settingsButton.onClick.AddListener(OnSettingsButton);
    }

    public void OnResumeButtonClicked()
    {
        GameManager.instance.PauseAndUnpauseGame();
    }

    public void OnSettingsButton()
    {
        AudioManager.instance.PlayButtonSound();
        InputManager.instance.playerInput.actions["Pause"].Disable();
        settingsUI.SetActive(true);
    }

    public void OnExitButtonClicked()
    {
        AudioManager.instance.PlayButtonSound();
        Time.timeScale = 1;      
        SceneManager.LoadSceneAsync("MainMenu");
    }
}

