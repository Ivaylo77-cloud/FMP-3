using UnityEngine;

public class OysterPickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OysterCard player = other.GetComponent<OysterCard>();

            if (player != null)
            {
                player.PickUpCard();
                gameObject.SetActive(false);
            }
        }
    }
}