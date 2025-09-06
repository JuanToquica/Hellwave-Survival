using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    //[SerializeField] private SettingsData settingsData;
    [SerializeField] private GameObject mainMenuUI;
    [SerializeField] private Button backButton;
    [SerializeField] private Slider volume;
    [SerializeField] private Button resetControlsSettings;

    private void Start()
    {
        backButton.onClick.AddListener(OnBackButtonClicked);
        //resetControlsSettings.onClick.AddListener(OnResetControlsClicked);
        LoadSliderSettins();
    }

    private void LoadSliderSettins()
    {
        volume.value = PlayerPrefs.GetFloat("MasterVolume");
    }

    public void OnBackButtonClicked()
    {
        mainMenuUI.SetActive(true);
        gameObject.SetActive(false);
    }

    public void OnVolumeChanged()
    {
        PlayerPrefs.SetFloat("MasterVolume", volume.value);
    }

    //public void OnResetControlsClicked()
    //{
    //    PlayerPrefs.SetFloat("Sensitivity", settingsData.defaultSensitivity);
    //    LoadSliderSettins();
    //    InputManager.Instance.ResetActionMaps();
    //    EventSystem.current.SetSelectedGameObject(null);
    //}


    private void OnEnable()
    {
        EventSystem.current.firstSelectedGameObject = volume.gameObject;
        LoadSliderSettins();
    }
}

