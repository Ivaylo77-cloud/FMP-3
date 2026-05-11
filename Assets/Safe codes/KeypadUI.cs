using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class KeypadUI : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text displayText;

    [Header("Code")]
    public string correctCode = "1234";

    [Header("Door")]
    public SafeDoor safeDoor;

    [Header("Sounds")]
    public AudioSource audioSource;

    public AudioClip buttonBeepSound;
    public AudioClip wrongCodeSound;
    public AudioClip correctCodeSound;

    private string currentInput = "";

    public void PressNumber(string number)
    {
        if (currentInput.Length < 4)
        {
            // Play keypad button sound
            PlaySound(buttonBeepSound);

            currentInput += number;
            displayText.text = currentInput;
        }
    }

    public void Confirm()
    {
        if (currentInput == correctCode)
        {
            // Play correct code sound
            PlaySound(correctCodeSound);

            displayText.text = "CORRECT!";

            safeDoor.OpenDoor();

            // Wait before closing UI
            Invoke(nameof(CloseUI), 1f);

            Invoke(nameof(ClearDisplay), 3f);
        }
        else
        {
            // Play wrong code sound
            PlaySound(wrongCodeSound);

            currentInput = "";
            displayText.text = "WRONG!";

            Invoke(nameof(ClearDisplay), 1f);
        }
    }

    void ClearDisplay()
    {
        displayText.text = "";
    }

    public void CloseUI()
    {
        gameObject.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        currentInput = "";
        displayText.text = "";
    }

    void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}