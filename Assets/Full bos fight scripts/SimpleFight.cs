using UnityEngine;
using UnityEngine.UI;

public class SimpleFight : MonoBehaviour
{
    public int playerHealth = 100;
    public int bossHealth = 100;

    public Slider playerSlider;
    public Slider bossSlider;

    public GameObject normalCamera;
    public GameObject fightCamera;

    public Animator playerAnimator;
    public Animator bossAnimator;

    public GameObject bossHPBar;
    public GameObject playerHPBar;

    public GameObject player;
    public GameObject boss;

    public MonoBehaviour playerMovement;

    public bool fightStarted = false;

    void Start()
    {
        playerSlider.maxValue = playerHealth;
        playerSlider.value = playerHealth;

        bossSlider.maxValue = bossHealth;
        bossSlider.value = bossHealth;
    }

    void Update()
    {
        if (!fightStarted)
            return;

        // Q attack
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Attack1();
        }

        // E attack
        if (Input.GetKeyDown(KeyCode.E))
        {
            Attack2();
        }
    }


    void Attack1()
    {
        Debug.Log("ATTACK 1 TRIGGERED");

        bossHealth -= 10;

        bossSlider.value = bossHealth;

        Debug.Log("BOSS HP AFTER HIT: " + bossHealth);

        CheckBossDeath();
    }

    void Attack2()
    {
        Debug.Log("ATTACK 2 TRIGGERED");

        bossHealth -= 20;

        bossSlider.value = bossHealth;

        Debug.Log("BOSS HP AFTER HIT: " + bossHealth);

        CheckBossDeath();
    }

    public void StartFight()
    {
        if (fightStarted) return;

        Debug.Log("Fight Started");

        fightStarted = true;

        Vector3 bossPos = boss.transform.position;
        bossPos.y = player.transform.position.y;

        player.transform.LookAt(bossPos);

        Vector3 playerPos = player.transform.position;
        playerPos.y = boss.transform.position.y;

        boss.transform.LookAt(playerPos);

        InvokeRepeating(nameof(BossAttack), 2f, 3f);
    }

    void BossAttack()
    {
        Debug.Log("BossAttack FROM: " + gameObject.name + " ID: " + GetInstanceID());

        Debug.Log("BEFORE DAMAGE: " + playerHealth);

        Debug.Log("BossAttack RUNNING");

        int randomAttack = Random.Range(0, 2);

        if (randomAttack == 0)
        {
            bossAnimator.SetTrigger("Attack1");

            playerHealth -= 10;
        }
        else
        {
            bossAnimator.SetTrigger("Attack2");

            playerHealth -= 15;
        }

        playerSlider.value = playerHealth;

        Debug.Log("AFTER DAMAGE: " + playerHealth);


        Debug.Log("Player HP: " + playerHealth);

        CheckPlayerDeath();
    }

    void CheckBossDeath()
    {
        if (bossHealth <= 0)
        {
            fightStarted = false;

            CancelInvoke();

            Debug.Log("Boss Defeated");

            // Switch cameras back
            fightCamera.SetActive(false);
            normalCamera.SetActive(true);

            // Re-enable movement
            if (playerMovement != null)
            {
                playerMovement.enabled = true;
            }

            // Stop player drifting
            Rigidbody rb = player.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }

            // Disable interaction so fight cannot restart
            Interaction interaction = boss.GetComponent<Interaction>();

            if (interaction != null)
            {
                interaction.enabled = false;
            }

            bossHPBar.SetActive(false);
            playerHPBar.SetActive(false);
        }
    }

    void CheckPlayerDeath()
    {
        if (playerHealth <= 0)
        {
            fightStarted = false;

            CancelInvoke(nameof(BossAttack));

            playerAnimator.SetTrigger("Die");

            Debug.Log("Player Died");
        }
    }

    void Awake()
    {
        Debug.Log("AWAKE SimpleFight ID: " + GetInstanceID());
    }

}
