using UnityEngine;

public class EndingTrigger : MonoBehaviour
{
    public GameObject endingImage;

    public GameObject mainMenuButton;
    public GameObject exitButton;

    public MonoBehaviour playerMovement;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // SHOW ENDING IMAGE
            endingImage.SetActive(true);

            // SHOW BUTTONS
            mainMenuButton.SetActive(true);
            exitButton.SetActive(true);

            // STOP PLAYER
            if (playerMovement != null)
            {
                playerMovement.enabled = false;
            }

            // SHOW CURSOR
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            // PAUSE GAME
            Time.timeScale = 0f;
        }
    }
}