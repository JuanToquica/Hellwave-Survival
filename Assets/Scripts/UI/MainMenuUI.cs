using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections;
using TMPro;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject settingsIU;
    [SerializeField] private SettingsUI settings;
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private TextMeshProUGUI highScore;

    private void OnEnable()
    {
        highScore.text = PlayerPrefs.GetInt("HighScore",0).ToString();
    }

    private void Start()
    {
        playButton.onClick.AddListener(OnPlayButtonClicked);
        settingsButton.onClick.AddListener(OnSettingsButtonClicked);
    }

    public void OnPlayButtonClicked()
    {
        AudioManager.instance.PlayButtonSound();
        SceneManager.LoadSceneAsync("GameScene");
    }

    public void OnSettingsButtonClicked()
    {
        AudioManager.instance.PlayButtonSound();
        settingsIU.SetActive(true);
    }
}
