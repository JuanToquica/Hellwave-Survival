using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject settingsIU;
    [SerializeField] private SettingsUI settings;
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingsButton;

    private void Start()
    {
        playButton.onClick.AddListener(OnPlayButtonClicked);
        settingsButton.onClick.AddListener(OnSettingsButtonClicked);
    }

    public void OnPlayButtonClicked()
    {
        SceneManager.LoadSceneAsync("GameScene");
    }

    public void OnSettingsButtonClicked()
    {
        settingsIU.SetActive(true);
        gameObject.SetActive(false);
    }
}
