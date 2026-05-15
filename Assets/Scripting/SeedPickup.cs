using UnityEngine;

public class SeedPickup : MonoBehaviour
{
    public SeedBar seedBar;

    public Interaction interaction;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            seedBar.AddSeed();

            interaction.AddBonusTime();

            Destroy(gameObject);
        }
    }
}