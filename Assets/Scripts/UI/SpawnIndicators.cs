using UnityEngine;
using System.Collections;

public class SpawnIndicators : MonoBehaviour
{
    [SerializeField] private Transform[] targets;
    [SerializeField] private Transform[] indicators;
    [SerializeField] private SpriteRenderer[] sprites;
    [SerializeField] private float spawnIndicatorTime;
    [SerializeField] private float fadeDuration;
    [SerializeField] private float defaultAlpha;

    private void Start()
    {
        DisableIndicators();
    }

    private void ShowIndicators(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            indicators[i].gameObject.SetActive(true);
            sprites[i].color = new Color(sprites[i].color.r, sprites[i].color.g, sprites[i].color.b, defaultAlpha);
        }
    }

    private void DisableIndicators()
    {
        for (int i = 0; i < indicators.Length; i++)
        {
            indicators[i].gameObject.SetActive(false);
        }
    }

    public void IndicateSpawns(int directions)
    {
        StartCoroutine(IndicateSpawnsCoroutine(directions));
    }

    private IEnumerator IndicateSpawnsCoroutine(int directions)
    {
        float timer = 0;
        if (directions > indicators.Length) directions = indicators.Length;
        ShowIndicators(directions);
        while (timer < spawnIndicatorTime)
        {
            for (int i = 0; i < directions; i++)
            {
                Vector3 direction = targets[i].position - indicators[i].position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                indicators[i].rotation = Quaternion.Euler(0, 0, angle);
            }
            timer += Time.deltaTime;
            yield return null;
        }
        StartCoroutine(HideIndicators(directions));
    }

    private IEnumerator HideIndicators(int amount)
    {
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;

            for (int i = 0; i < amount; i++)
            {
                float alpha = Mathf.SmoothStep(defaultAlpha, 0f, timer / fadeDuration);
                sprites[i].color = new Color(sprites[i].color.r, sprites[i].color.g, sprites[i].color.b, alpha);
            }           
            yield return null;
        }

        DisableIndicators();
    }
}
