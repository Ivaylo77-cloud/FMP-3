using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SimpleFight : MonoBehaviour
{
    public int playerHealth = 100;
    public int bossHealth = 100;

    public Slider playerSlider;
    public Slider bossSlider;

    [Header("Attack Cooldowns")]

    public Slider attack1CooldownSlider;
    public Slider attack2CooldownSlider;

    public float attack1Cooldown = 2f;
    public float attack2Cooldown = 5f;

    private bool canAttack1 = true;
    private bool canAttack2 = true;

    public GameObject normalCamera;
    public GameObject fightCamera;
    public GameObject doorCutsceneCamera;

    public Animator playerAnimator;
    public Animator bossAnimator;
    public Animator metroDoorAnimator;

    public GameObject bossHPBar;
    public GameObject playerHPBar;

    public GameObject player;
    public GameObject boss;

    public MonoBehaviour playerMovement;

    public bool fightStarted = false;

    private CameraShake cameraShake;

    [Header("Sound Effects")]

    public AudioSource sfxSource;

    public AudioClip playerAttack1Sound;
    public AudioClip playerAttack2Sound;

    public AudioClip bossAttack1Sound;
    public AudioClip bossAttack2Sound;

    public AudioClip doorOpenSound;

    public AudioClip playerDeathSound;
    public AudioClip bossDeathSound;

    [Header("Death Screen")]

    public GameObject deathScreen;



    void Start()
    {
        playerSlider.maxValue = playerHealth;
        playerSlider.value = playerHealth;

        bossSlider.maxValue = bossHealth;
        bossSlider.value = bossHealth;

        attack1CooldownSlider.value = 1;
        attack2CooldownSlider.value = 1;

        attack1CooldownSlider.gameObject.SetActive(false);
        attack2CooldownSlider.gameObject.SetActive(false);

        cameraShake = fightCamera.GetComponent<CameraShake>();

        
    }

    void Update()
    {
        if (!fightStarted)
            return;

        // Q attack
        if (Input.GetKeyDown(KeyCode.Q) && canAttack1)
        {
            Attack1();
        }

        // E attack
        if (Input.GetKeyDown(KeyCode.E) && canAttack2)
        {
            Attack2();
        }
    }


    void Attack1()
    {
        playerAnimator.SetTrigger("Attack1");

        sfxSource.PlayOneShot(playerAttack1Sound);

        Debug.Log("ATTACK 1 TRIGGERED");

        bossHealth -= 8;

        bossSlider.value = bossHealth;

        StartCoroutine(cameraShake.Shake(0.15f, 0.15f));

        Debug.Log("BOSS HP AFTER HIT: " + bossHealth);

        CheckBossDeath();

        StartCoroutine(Attack1Cooldown());
    }

    void Attack2()
    {
        playerAnimator.SetTrigger("Attack2");

        sfxSource.PlayOneShot(playerAttack2Sound);

        Debug.Log("ATTACK 2 TRIGGERED");

        bossHealth -= 15;

        bossSlider.value = bossHealth;

        StartCoroutine(cameraShake.Shake(0.15f, 0.15f));

        Debug.Log("BOSS HP AFTER HIT: " + bossHealth);

        CheckBossDeath();

        StartCoroutine(Attack2Cooldown());
    }

    IEnumerator Attack1Cooldown()
    {
        canAttack1 = false;

        float timer = attack1Cooldown;

        attack1CooldownSlider.gameObject.SetActive(true);

        while (timer > 0)
        {
            timer -= Time.deltaTime;

            attack1CooldownSlider.value = 1 - (timer / attack1Cooldown);

            yield return null;
        }

        attack1CooldownSlider.value = 1;

        canAttack1 = true;
    }

    IEnumerator Attack2Cooldown()
    {
        canAttack2 = false;

        float timer = attack2Cooldown;

        attack2CooldownSlider.gameObject.SetActive(true);

        while (timer > 0)
        {
            timer -= Time.deltaTime;

            attack2CooldownSlider.value = 1 - (timer / attack2Cooldown);

            yield return null;
        }

        attack2CooldownSlider.value = 1;

        canAttack2 = true;
    }

    public void StartFight()
    {
        if (fightStarted) return;

        Debug.Log("Fight Started");

        fightStarted = true;

        attack1CooldownSlider.gameObject.SetActive(true);
        attack2CooldownSlider.gameObject.SetActive(true);

        // STOP PATROL SCRIPT
        BossPatrol patrol = boss.GetComponent<BossPatrol>();

        if (patrol != null)
        {
            patrol.enabled = false;
        }

        // STOP WALKING ANIMATION
        bossAnimator.SetBool("IsWalking", false);

        // FACE EACH OTHER
        Vector3 bossPos = boss.transform.position;
        bossPos.y = player.transform.position.y;

        player.transform.LookAt(bossPos);

        Vector3 playerPos = player.transform.position;
        playerPos.y = boss.transform.position.y;

        boss.transform.LookAt(playerPos);

        // START BOSS ATTACKS
        InvokeRepeating(nameof(BossAttack), 2f, 3f);
    }

    void BossAttack()
    {
        Debug.Log("BossAttack RUNNING");

        int randomAttack = Random.Range(0, 2);

        if (randomAttack == 0)
        {
            bossAnimator.SetTrigger("Attack1");

            Invoke(nameof(PlayBossAttack1Sound), 0.7f);

            Invoke(nameof(BossAttack1Damage), 1.2f);
        }
        else
        {
            bossAnimator.SetTrigger("Attack2");

            Invoke(nameof(PlayBossAttack2Sound), 1.8f);

            Invoke(nameof(BossAttack2Damage), 2f);
        }
    }

    void PlayBossAttack1Sound()
    {
        sfxSource.PlayOneShot(bossAttack1Sound);
    }

    void PlayBossAttack2Sound()
    {
        sfxSource.PlayOneShot(bossAttack2Sound);
    }

    void BossAttack1Damage()
    {
        playerAnimator.SetTrigger("Hurt");

        playerHealth -= 15;

        playerSlider.value = playerHealth;

        StartCoroutine(cameraShake.Shake(0.2f, 0.2f));

        Debug.Log("Attack1 HIT");

        CheckPlayerDeath();
    }

    void BossAttack2Damage()
    {
        playerAnimator.SetTrigger("Hurt");

        playerHealth -= 20;

        playerSlider.value = playerHealth;

        StartCoroutine(cameraShake.Shake(0.2f, 0.2f));

        Debug.Log("Attack2 HIT");

        CheckPlayerDeath();
    }

    void CheckBossDeath()
    {
        if (bossHealth <= 0)
        {
            fightStarted = false;

            CancelInvoke();

            Debug.Log("Boss Defeated");

            Invoke(nameof(PlayBossDeathSound), 1f);

            // PLAY DEATH ANIMATION
            bossAnimator.SetTrigger("Die");

            // STOP PATROL
            BossPatrol patrol = boss.GetComponent<BossPatrol>();

            if (patrol != null)
            {
                patrol.enabled = false;
            }

            // DISABLE INTERACTION
            Interaction interaction = boss.GetComponent<Interaction>();

            if (interaction != null)
            {
                interaction.enabled = false;
            }

            // OPTIONAL: disable collider
            Collider bossCollider = boss.GetComponent<Collider>();

            if (bossCollider != null)
            {
                bossCollider.enabled = false;
            }

            // WAIT BEFORE HIDING UI/CAMERA
            StartCoroutine(BossDeathSequence());
        }
    }

    IEnumerator BossDeathSequence()
    {
        // wait for death animation
        yield return new WaitForSeconds(3f);

        // SWITCH TO CUTSCENE CAMERA
        fightCamera.SetActive(false);
        doorCutsceneCamera.SetActive(true);

        // OPEN METRO DOORS
        metroDoorAnimator.SetTrigger("OpenDoors");

        sfxSource.PlayOneShot(doorOpenSound);

        // WAIT FOR CUTSCENE
        yield return new WaitForSeconds(4f);

        // RETURN TO NORMAL CAMERA
        doorCutsceneCamera.SetActive(false);
        normalCamera.SetActive(true);

        // enable player movement
        if (playerMovement != null)
        {
            playerMovement.enabled = true;
        }

        // stop player drifting
        Rigidbody rb = player.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        // hide combat UI
        bossHPBar.SetActive(false);
        playerHPBar.SetActive(false);

        attack1CooldownSlider.gameObject.SetActive(false);
        attack2CooldownSlider.gameObject.SetActive(false);
    }

    void CheckPlayerDeath()
    {
        if (playerHealth <= 0)
        {
            fightStarted = false;

            attack1CooldownSlider.gameObject.SetActive(false);
            attack2CooldownSlider.gameObject.SetActive(false);

            CancelInvoke(nameof(BossAttack));

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            Invoke(nameof(PlayPlayerDeathSound), 1f);

            StartCoroutine(PlayerDeathSequence());

            // fake falling over
            player.transform.rotation = Quaternion.Euler(90f, 0f, 0f);

            Debug.Log("Player Died");
        }
    }

    void PlayPlayerDeathSound()
    {
        sfxSource.PlayOneShot(playerDeathSound, 2f);
    }

    void PlayBossDeathSound()
    {
        sfxSource.PlayOneShot(bossDeathSound, 2f);
    }

    IEnumerator PlayerDeathSequence()
    {
        // small delay
        yield return new WaitForSeconds(2f);

        // show death screen
        deathScreen.SetActive(true);
    }

    public void RestartFight()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    void Awake()
    {
        Debug.Log("AWAKE SimpleFight ID: " + GetInstanceID());
    }

}
