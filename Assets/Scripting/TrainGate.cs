using UnityEngine;

public class TrainGate : MonoBehaviour
{
    public Transform gate; // the barrier
    public Vector3 openRotation = new Vector3(0, 90, 0);

    private bool playerInRange = false;
    private OysterCard player;

    void Update()
    {
        if (playerInRange && player != null)
        {
            if (Input.GetKeyDown(KeyCode.E) && player.hasOysterCard)
            {
                OpenGate();
                player.UseCard();
            }
        }
    }

    void OpenGate()
    {
        // simple open (you can animate later)
        gate.localRotation = Quaternion.Euler(openRotation);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            player = other.GetComponent<OysterCard>();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            player = null;
        }
    }
}