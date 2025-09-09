using UnityEngine;
using System.Collections;

public class EnemySoundControler : MonoBehaviour
{
    private Coroutine growlRoutine;

    void OnBecameVisible()
    {
        if (growlRoutine == null)
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
}
