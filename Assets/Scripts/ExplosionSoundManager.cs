using UnityEngine;

public class ExplosionSoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource[] explosionsSources;
    private int explosionCurrentlyPlaying;

    public void PlayExplosionSound()
    {
        if (explosionCurrentlyPlaying >= explosionsSources.Length)
        {
            explosionCurrentlyPlaying = 0;
            explosionsSources[0].Stop();
        }
        explosionsSources[explosionCurrentlyPlaying].Play();
        explosionCurrentlyPlaying++;
    }
}
