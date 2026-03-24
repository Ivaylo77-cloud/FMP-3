using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Interaction : MonoBehaviour
{
    [Header("UI")]
    public GameObject interactText;
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;

    [Header("Dialogue")]
    [TextArea]
    public string[] dialogueLines;
    public float typingSpeed = 0.03f;

    private bool isPlayerNearby = false;
    private bool isTalking = false;

    private int currentLine = 0;
    private bool isTyping = false;
    private Coroutine typingCoroutine;

    void Start()
    {
        interactText.SetActive(false);
        dialoguePanel.SetActive(false);
    }

    void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.F))
        {
            if (!isTalking)
            {
                StartDialogue();
            }
            else
            {
                if (isTyping)
                {
                    // Skip typing
                    StopCoroutine(typingCoroutine);
                    dialogueText.text = dialogueLines[currentLine];
                    isTyping = false;
                }
                else
                {
                    NextLine();
                }
            }
        }
    }

    void StartDialogue()
    {
        isTalking = true;
        currentLine = 0;
        dialoguePanel.SetActive(true);
        interactText.SetActive(false);

        StartTyping();
    }

    void NextLine()
    {
        currentLine++;

        if (currentLine >= dialogueLines.Length)
        {
            EndDialogue();
        }
        else
        {
            StartTyping();
        }
    }

    void StartTyping()
    {
        typingCoroutine = StartCoroutine(TypeLine(dialogueLines[currentLine]));
    }

    IEnumerator TypeLine(string line)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char letter in line)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    void EndDialogue()
    {
        isTalking = false;
        dialoguePanel.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            interactText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            interactText.SetActive(false);
            EndDialogue();
        }
    }
}