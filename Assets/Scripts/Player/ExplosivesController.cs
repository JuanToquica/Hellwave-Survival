using UnityEngine;

public class ExplosivesController : MonoBehaviour
{
    public void Explode()
    {
        Destroy(gameObject);
    }
}
