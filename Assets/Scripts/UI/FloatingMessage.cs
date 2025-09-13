using System.Collections;
using TMPro;
using UnityEngine;

public class FloatingMessage : MonoBehaviour
{
    public float duration;
    public float moveUpSpeed;

    private TextMeshProUGUI text;
    private Color originalColor;

    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        originalColor = text.color;
    }

    public void Show(string message)
    {
        text.text = message;
        StartCoroutine(MoveAndFadeAway());
    }

    private IEnumerator MoveAndFadeAway()
    {
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;

            transform.Translate(Vector3.up * moveUpSpeed * Time.deltaTime);

            float alpha = Mathf.Lerp(1f, 0f, timer / duration);
            text.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            yield return null;
        }

        Destroy(gameObject);
    }
}
