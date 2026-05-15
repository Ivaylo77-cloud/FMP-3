using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Interaction : MonoBehaviour
{
    [Header("Boss Fight")]
    public bool isBoss = true;

    public SimpleFight simpleFight;

    public Slider playerSlider;
    public Slider bossSlider;

    public GameObject bossHPBar;
    public GameObject playerHPBar;

    public MonoBehaviour bossPatrol;

    public GameObject normalCamera;
    public GameObject fightCamera;

    public MonoBehaviour playerMovement;

    [Header("FINAL NPC (SEEDS)")]
    public bool isFinalNPC = false;

    public SeedBar seedBar;
    public TMP_Text resultText;
    public GameObject loseImage;
    public GameObject fakeDoor;

    public TMP_Text loseText;
    public GameObject retryButton;
    public GameObject mainMenuButton;

    [Header("UI")]
    public GameObject interactText;
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;

    [Header("Dialogue")]
    [TextArea]
    public string[] dialogueLines;
    public float typingSpeed = 0.03f;

    [Header("Sound")]
    public AudioSource audioSource;
    public AudioClip talkingSound;

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
                    StopCoroutine(typingCoroutine);
                    dialogueText.text = dialogueLines[currentLine];
                    isTyping = false;
                    audioSource.Stop();
                    audioSource.loop = false;
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

        audioSource.pitch = Random.Range(0.95f, 1.05f);

        if (talkingSound != null)
        {
            audioSource.clip = talkingSound;
            audioSource.loop = true;
            audioSource.Play();
        }

        foreach (char letter in line)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;

        audioSource.Stop();
        audioSource.loop = false;
    }

    void EndDialogue()
    {
        isTalking = false;

        dialoguePanel.SetActive(false);
        interactText.SetActive(false);

        audioSource.Stop();
        audioSource.loop = false;

        // =========================
        // 🟡 FINAL NPC LOGIC HERE
        // =========================
        if (isFinalNPC)
        {
            int collected = seedBar.currentSeeds;
            int needed = seedBar.maxSeeds;

            resultText.gameObject.SetActive(true);

            if (collected >= needed)
            {
                fakeDoor.SetActive(false);

                resultText.text =
                    collected + " / " + needed +
                    " Seeds\n\nYou can fly home now.";
            }
            else
            {
                loseImage.SetActive(true);

                loseText.gameObject.SetActive(true);
                retryButton.SetActive(true);
                mainMenuButton.SetActive(true);

                loseText.text =
                    "You Lost due to not collecting all seeds\n\n" +
                    "You collected " + collected + " / " + needed;

                Time.timeScale = 0f;
            }

            return; // IMPORTANT: stops boss logic
        }

        // =========================
        // 🔴 BOSS LOGIC (UNCHANGED)
        // =========================
        if (isBoss)
        {
            if (simpleFight.fightStarted)
                return;

            bossPatrol.enabled = false;
            playerMovement.enabled = false;

            normalCamera.SetActive(false);
            fightCamera.SetActive(true);

            bossHPBar.SetActive(true);
            playerHPBar.SetActive(true);

            simpleFight.StartFight();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            interactText.SetActive(true);

            if (isBoss && bossPatrol != null)
            {
                bossPatrol.enabled = false;
            }

            Animator bossAnimator = GetComponent<Animator>();

            if (bossAnimator != null)
            {
                bossAnimator.SetBool("IsWalking", false);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            interactText.SetActive(false);

            if (!isTalking && isBoss && bossPatrol != null)
            {
                bossPatrol.enabled = true;
            }

            Animator bossAnimator = GetComponent<Animator>();

            if (bossAnimator != null)
            {
                bossAnimator.SetBool("IsWalking", true);
            }
        }
    }
}