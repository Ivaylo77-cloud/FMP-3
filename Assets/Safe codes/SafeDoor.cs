using UnityEngine;

public class SafeDoor : MonoBehaviour
{
    public Animator animator;

    private bool isOpen = false;

    public void OpenDoor()
    {
        if (!isOpen)
        {
            animator.SetTrigger("Open");
            isOpen = true;
        }
    }
}