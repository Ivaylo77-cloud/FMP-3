using UnityEngine;

public class SafeDoor : MonoBehaviour
{
    public Animator animator;

    private bool isOpen = false;

    public void OpenDoor()
    {
        if (!isOpen)
        {
            Destroy(gameObject);
            isOpen = true;
        }
    }
}