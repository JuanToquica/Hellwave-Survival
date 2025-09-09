using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class GrowlSoundManager : MonoBehaviour
{
    public static GrowlSoundManager instance;
    [SerializeField] private AudioClip[] enemyGrowlSounds;
    [SerializeField] private int maxConcurrentGrowls;
    private int growlsCurrentlyPlaying;

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
    }

    public void RequestGrowlSound()
    {
        if (growlsCurrentlyPlaying >= maxConcurrentGrowls)
        {
            return;
        }
        int index = Random.Range(0, enemyGrowlSounds.Length);
        StartCoroutine(PlayAndTrackGrowl(enemyGrowlSounds[index]));
    }

    private IEnumerator PlayAndTrackGrowl(AudioClip clip)
    {
        growlsCurrentlyPlaying++;
        AudioManager.instance.PlayEnemyGrowlSound(clip);
        yield return new WaitForSeconds(clip.length);
        growlsCurrentlyPlaying--;
    }
}
