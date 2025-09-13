using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    private AudioSource audioSource;
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
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        audioSource.volume = PlayerPrefs.GetFloat("Volume");
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
}
