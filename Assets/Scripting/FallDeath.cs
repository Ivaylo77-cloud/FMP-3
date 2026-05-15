using TMPro;
using UnityEngine;

public class FallDeath : MonoBehaviour
{
    public GameObject loseImage;

    public TMP_Text loseText;

    public GameObject retryButton;
    public GameObject mainMenuButton;

    public MonoBehaviour playerMovement;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            loseImage.SetActive(true);

            loseText.gameObject.SetActive(true);

            retryButton.SetActive(true);
            mainMenuButton.SetActive(true);

            loseText.text = "HAHA You Died From Fall Damage";

            // stop movement
            if (playerMovement != null)
            {
                playerMovement.enabled = false;
            }

            // show mouse
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            Time.timeScale = 0f;
        }
    }
}