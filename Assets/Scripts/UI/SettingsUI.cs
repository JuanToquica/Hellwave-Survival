using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [SerializeField] private Button backButton;
    [SerializeField] private Slider sfxVolume;
    [SerializeField] private Slider musicVolume;
    [SerializeField] private Button resetControlsSettings;

    private void Start()
    {
        backButton.onClick.AddListener(OnBackButtonClicked);
        resetControlsSettings.onClick.AddListener(OnResetControlsClicked);
        LoadVolumeSettins();
    }

    private void LoadVolumeSettins()
    {
        sfxVolume.value = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
        musicVolume.value = PlayerPrefs.GetFloat("MusicVolume", 0.2f);
    }

    public void OnBackButtonClicked()
    {
        AudioManager.instance.PlayButtonSound();
        InputManager.instance.playerInput.actions["Pause"].Enable();
        gameObject.SetActive(false);
    }

    public void OnSFXVolumeChanged()
    {
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume.value);
        AudioManager.instance.SetSFXVolume(sfxVolume.value);
    }

    public void OnMusicVolumeChanged()
    {
        PlayerPrefs.SetFloat("MusicVolume", musicVolume.value);
        AudioManager.instance.SetMusicVolume(musicVolume.value);
    }

    [ContextMenu("Eliminar player prefbs")]
    public void EliminarKeys()
    {
        PlayerPrefs.DeleteAll();
    }

    public void OnResetControlsClicked()
    {
        AudioManager.instance.PlayButtonSound();
        InputManager.instance.ResetActionMaps();
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void OnRebindButton()
    {
        AudioManager.instance.PlayButtonSound();
        EventSystem.current.SetSelectedGameObject(null);
    }


    private void OnEnable()
    {
        LoadVolumeSettins();
    }

    private void OnDisable()
    {
        PlayerPrefs.Save();
    }
}

