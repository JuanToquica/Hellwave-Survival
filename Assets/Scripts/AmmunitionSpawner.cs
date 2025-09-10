using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class AmmunitionSpawner : MonoBehaviour
{
    [SerializeField] private GameObject ammunitionBoxPrefab;
    [SerializeField] private Transform[] spawners;
    [SerializeField] private PlayerAttackManager player;
    [SerializeField] private float minTimeToSpawn;
    [SerializeField] private float maxTimeToSpawn;

    private void Start()
    {
        for (int i = 0; i < spawners.Length; i++)
        {
            StartCoroutine(spawnAmmunitionBox(i));
        }
    }

    public void BoxCollected(int spawnIndex)
    {
        StartCoroutine(spawnAmmunitionBox(spawnIndex));
    }

    private IEnumerator spawnAmmunitionBox(int spawnIndex)
    {
        yield return new WaitForSeconds(Random.Range(minTimeToSpawn, maxTimeToSpawn));
        AmmunitionBox box = ObjectPoolManager.instance.GetPooledObject(ammunitionBoxPrefab, spawners[spawnIndex].position, spawners[spawnIndex].rotation).
            GetComponent<AmmunitionBox>();
        box.spawnIndex = spawnIndex;
        box.spawner = this;
        box.player = player;
    }
}
