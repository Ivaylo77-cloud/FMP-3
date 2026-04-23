using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeypadUI : MonoBehaviour
{
    public Text displayText;
    public string correctCode = "1234";

    private string currentInput = "";

    public SafeDoor safeDoor;

    public void PressNumber(string number)
    {
        if (currentInput.Length < 4)
        {
            currentInput += number;
            displayText.text = currentInput;
        }
    }

    public void Confirm()
    {
        if (currentInput == correctCode)
        {
            safeDoor.OpenDoor();
            CloseUI();
        }
        else
        {
            currentInput = "";
            displayText.text = "WRONG";
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
}