using UnityEngine;

public class AmmunitionBox : MonoBehaviour
{
    public AmmunitionSpawner spawner;
    public PlayerAttackManager player;
    public int spawnIndex;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            AudioManager.instance.PlayReloadSound();
            spawner.BoxCollected(spawnIndex);
            player.CollectAmmunition();
            ObjectPoolManager.instance.ReturnPooledObject(gameObject);
        }
    }
}
