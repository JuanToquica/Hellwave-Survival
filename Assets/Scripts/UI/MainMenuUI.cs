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

        //if (!PlayerPrefs.HasKey("MasterVolume") || !PlayerPrefs.HasKey("MusicVolume") || !PlayerPrefs.HasKey("SFXVolume"))
        //{
        //    settingsIU.SetActive(true);
        //    settings.OnResetVolumeClicked();
        //    settingsIU.SetActive(false);
        //}
        //if (!PlayerPrefs.HasKey("Sensitivity"))
        //{
        //    settingsIU.SetActive(true);
        //    settings.OnResetControlsClicked();
        //    settingsIU.SetActive(false);
        //}

        StartCoroutine(Deselect());
    }

    IEnumerator Deselect()
    {
        yield return null;
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void OnPlayButtonClicked()
    {
        StartCoroutine(LoadAsync("GameScene"));
    }

    public void OnSettingsButtonClicked()
    {
        settingsIU.SetActive(true);
        gameObject.SetActive(false);
    }

    private IEnumerator LoadAsync(string scene)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(scene);
        while (!asyncOperation.isDone)
        {
            Debug.Log(asyncOperation.progress);
            yield return null;
        }
    }
    public void OnEnable()
    {
        EventSystem.current.firstSelectedGameObject = playButton.gameObject;
    }
}
