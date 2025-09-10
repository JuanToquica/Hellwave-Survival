using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed;
    public Vector2 maxXPosition;
    public Vector2 maxYPosition;
    private Vector3 shakeOffset;
    [SerializeField] private float shakeDuration;
    [SerializeField] private float shakeMagnitude;
    private Coroutine shakeCoroutine;

    private void OnEnable()
    {
        shakeOffset = Vector3.zero;
        ExplosivesController.OnExplosion += StartShake;
    }

    private void OnDisable()
    {
        ExplosivesController.OnExplosion -= StartShake;
    }

    private void LateUpdate()
    {
        float targetXPosition = transform.position.x;
        float targetYPosition = transform.position.y;

        if (player.position.x < maxXPosition.y && player.position.x > maxXPosition.x)
            targetXPosition = player.position.x;
        else
            targetXPosition = player.position.x <= maxXPosition.x? maxXPosition.x : maxXPosition.y;

        if (player.position.y < maxYPosition.y && player.position.y > maxYPosition.x)
            targetYPosition = player.position.y;
        else
            targetYPosition = player.position.y <= maxYPosition.x ? maxYPosition.x : maxYPosition.y;

        Vector3 newPosition = new Vector3(targetXPosition, targetYPosition, -10);
        transform.position = Vector3.Lerp(transform.position, newPosition, smoothSpeed * Time.deltaTime) + shakeOffset;
    }

    [ContextMenu("start shake")]
    public void StartShake()
    {
        if (shakeCoroutine != null)
            StopCoroutine(shakeCoroutine);
        shakeCoroutine = StartCoroutine(Shake(shakeDuration, shakeMagnitude));      
    }

    public IEnumerator Shake(float duration, float magnitude)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            shakeOffset = new Vector3(x, y, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }
        shakeOffset = Vector3.zero;
    }
}
