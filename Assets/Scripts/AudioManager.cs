using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] private ExplosionSoundManager explosionSoundManager;
    [SerializeField] private AudioSource generalAudioSource;
    [SerializeField] private AudioSource growlsSource;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioClip buttonSound;
    [SerializeField] private AudioClip deploySound;
    [SerializeField] private AudioClip reloadSound;
    [SerializeField] private AudioClip shotgunReload;
    [SerializeField] private AudioClip explosionSound;
    [SerializeField] private AudioClip[] fireBallLaunchSound;
    [SerializeField] private AudioClip fireBallImpactSound;
    [SerializeField] private AudioClip dryFireSound;
    [SerializeField] private AudioClip playerDeathSound;
    [SerializeField] private AudioClip[] weaponShotSounds;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        generalAudioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        LoadVolume();
        PlayMusic();
    }

    private void LoadVolume()
    {
        SetMusicVolume(PlayerPrefs.GetFloat("MusicVolume", 0.2f));
        SetSFXVolume(PlayerPrefs.GetFloat("SFXVolume", 0.5f));
    }

    public void SetMusicVolume(float value)
    {
        float v = Mathf.Clamp(value, 0.0001f, 1f);
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(v) * 20);
    }

    public void SetSFXVolume(float value)
    {
        float v = Mathf.Clamp(value, 0.0001f, 1f);
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(v) * 20);
    }

    public void PlayPlayerShotSound(int weapon)
    {
        int index = weapon;
        if (index == 7) index = 4; //para reproducir sonido de la bazuca
        generalAudioSource.PlayOneShot(weaponShotSounds[index]);
        if (index == 1)
            Invoke("PlayShotgunReloadSound", 0.22f);
    }

    public void PlayEnemyGrowlSound(AudioClip clip)
    {
        growlsSource.PlayOneShot(clip);
    }

    public void PlayDeployWeaponSound()
    {
        generalAudioSource.PlayOneShot(deploySound);
    }

    public void PlayButtonSound()
    {
        generalAudioSource.PlayOneShot(buttonSound);
    }

    public void PlayReloadSound()
    {
        generalAudioSource.PlayOneShot(reloadSound);
    }

    private void PlayShotgunReloadSound()
    {
        generalAudioSource.PlayOneShot(shotgunReload);
    }

    public void PlayFireBallImpactSound()
    {
        generalAudioSource.PlayOneShot(fireBallImpactSound);
    }

    public void PlayDryFireSound()
    {
        generalAudioSource.PlayOneShot(dryFireSound);
    }

    public void PlayFireballLaunchSound()
    {
        generalAudioSource.PlayOneShot(fireBallLaunchSound[Random.Range(0,2)]);
    }

    public void PlayPlayerDeathSound()
    {
        generalAudioSource.PlayOneShot(playerDeathSound);
    }

    public void PlayMusic()
    {
        if (!musicSource.isPlaying)
            musicSource.Play();
    }

    public void PlayExplosionSound()
    {
        explosionSoundManager.PlayExplosionSound();
    }
}
