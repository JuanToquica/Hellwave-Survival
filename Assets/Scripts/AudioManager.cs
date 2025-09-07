using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    private AudioSource audioSource;
    [SerializeField] private AudioClip buttonSound;
    [SerializeField] private AudioClip enemyMeeleAttackSound;
    [SerializeField] private AudioClip enemyShooterAttackSound;
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
        audioSource.PlayOneShot(weaponShotSounds[weapon]);
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
}
