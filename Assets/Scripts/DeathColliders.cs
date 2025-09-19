using UnityEngine;

public class DeathColliders : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Enemy"))
        {
            EnemyHealth enemy = collision.transform.GetComponent<EnemyHealth>();
            enemy.OnLeavingMap();
        }
        else if (collision.transform.CompareTag("Player"))
        {
            PlayerHealth player = collision.transform.GetComponent<PlayerHealth>();
            player.OnLeavingMap();
        }
    }
}
