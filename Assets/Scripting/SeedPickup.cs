using UnityEngine;

public class SeedPickup : MonoBehaviour
{
    public SeedBar seedBar;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            seedBar.CollectSeed();
            Destroy(gameObject);
        }
    }
}
