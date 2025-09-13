using UnityEngine;
using System.Collections;

public class EnemySoundControler : MonoBehaviour
{
    private Coroutine growlRoutine;
    private bool gameFinished;

    private void OnEnable()
    {
        gameFinished = false;
        PlayerHealth.OnPlayerDeath += StopGrowling;
    }      
    private void OnDisable() => PlayerHealth.OnPlayerDeath -= StopGrowling;

    void OnBecameVisible()
    {
        if (growlRoutine == null && !gameFinished)
        {
            growlRoutine = StartCoroutine(PlayGrowl());
        }
    }
    void OnBecameInvisible()
    {
        if (growlRoutine != null)
        {
            StopCoroutine(growlRoutine);
            growlRoutine = null;
        }
    }

    IEnumerator PlayGrowl()
    {
        while (true)
        {
            GrowlSoundManager.instance.RequestGrowlSound();
            yield return new WaitForSeconds(6);
        }
    }

    private void StopGrowling()
    {
        gameFinished = true;
        if (growlRoutine != null)
            StopCoroutine(growlRoutine);
        this.enabled = false;
    }
}
