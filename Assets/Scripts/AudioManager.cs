using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] private AudioSource audioSource;
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
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        LoadVolume();
    }

    private void LoadVolume()
    {
        SetMusicVolume(PlayerPrefs.GetFloat("MusicVolume", 0.5f));
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
        audioSource.PlayOneShot(weaponShotSounds[index]);
        if (index == 1)
            Invoke("PlayShotgunReloadSound", 0.22f);
    }

    public void PlayEnemyGrowlSound(AudioClip clip)
    {        
        audioSource.PlayOneShot(clip);
    }

    public void PlayDeployWeaponSound()
    {
        audioSource.PlayOneShot(deploySound);
    }

    public void PlayButtonSound()
    {
        audioSource.PlayOneShot(buttonSound);
    }

    public void PlayReloadSound()
    {
        audioSource.PlayOneShot(reloadSound);
    }

    public void PlayExplosionSound()
    {
        audioSource.PlayOneShot(explosionSound);
    }

    private void PlayShotgunReloadSound()
    {
        audioSource.PlayOneShot(shotgunReload);
    }

    public void PlayFireBallImpactSound()
    {
        audioSource.PlayOneShot(fireBallImpactSound);
    }

    public void PlayDryFireSound()
    {
        audioSource.PlayOneShot(dryFireSound);
    }

    public void PlayFireballLaunchSound()
    {
        audioSource.PlayOneShot(fireBallLaunchSound[Random.Range(0,2)]);
    }

    public void PlayPlayerDeathSound()
    {
        audioSource.PlayOneShot(playerDeathSound);
    }

    public void PlayMusic()
    {
        if (!musicSource.isPlaying)
            musicSource.Play();
    }

    public void StopMusic()
    {
        StartCoroutine(MusicFadeOut());
    }

    public IEnumerator MusicFadeOut()
    {
        float startVolume = musicSource.volume;

        while (musicSource.volume > 0)
        {
            musicSource.volume -= startVolume * Time.deltaTime / 2;
            yield return null;
        }

        musicSource.Stop();
        musicSource.volume = startVolume;
    }
}
