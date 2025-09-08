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
    [SerializeField] private AudioClip enemyMeeleAttackSound;
    [SerializeField] private AudioClip enemyShooterAttackSound;
    [SerializeField] private AudioClip[] weaponShotSounds;
    public float time;
    
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
            Invoke("PlayShotgunReloadSound", time);
    }

    public void PlayDeployWeaponSound()
    {
        audioSource.PlayOneShot(deploySound);
    }

    public void PlayEnemyMeeleAttackSound()
    {
        audioSource.PlayOneShot(enemyMeeleAttackSound);
    }

    public void PlayEnemyShooterAttackSound()
    {
        audioSource.PlayOneShot(enemyShooterAttackSound);
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
}
