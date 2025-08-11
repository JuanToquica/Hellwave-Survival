using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed;
    public Vector2 maxXPosition;
    public Vector2 maxYPosition;

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
        transform.position = Vector3.Lerp(transform.position, newPosition, smoothSpeed * Time.deltaTime);
    }
}
