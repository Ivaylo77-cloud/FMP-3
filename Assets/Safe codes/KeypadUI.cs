using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class KeypadUI : MonoBehaviour
{
    public TMP_Text displayText;
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
            displayText.text = "CORRECT!";
            Invoke(nameof(ClearDisplay), 3f);
            safeDoor.OpenDoor();
            CloseUI();
            
        }
        else
        {
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

    
}