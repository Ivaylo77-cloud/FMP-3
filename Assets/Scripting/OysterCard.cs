using UnityEngine;


public class OysterCard : MonoBehaviour
{
    public bool hasOysterCard = false;
    public GameObject uiPanel;

    public void PickUpCard()
    {
        hasOysterCard = true;
        uiPanel.SetActive(true);
    }

    public void UseCard()
    {
        hasOysterCard = false;
        uiPanel.SetActive(false);
    }
}

