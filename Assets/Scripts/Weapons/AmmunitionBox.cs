using UnityEngine;

public class AmmunitionBox : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            AudioManager.instance.PlayReloadSound();
            Destroy(gameObject);
        }
    }
}
