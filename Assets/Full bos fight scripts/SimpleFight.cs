using UnityEngine;

public class SimpleFight : MonoBehaviour
{
    public int playerHealth = 100;
    public int bossHealth = 100;

    public Animator playerAnimator;
    public Animator bossAnimator;

    public GameObject player;
    public GameObject boss;

    public bool fightStarted = false;

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

        bossHealth = bossHealth - 10;

        Debug.Log("BOSS HP AFTER HIT: " + bossHealth);
    }

    void Attack2()
    {
        Debug.Log("ATTACK 2 TRIGGERED");

        bossHealth = bossHealth - 20;

        Debug.Log("BOSS HP AFTER HIT: " + bossHealth);
    }

    public void StartFight()
    {
        Debug.Log("Fight Started");

        fightStarted = true;

        player.transform.LookAt(boss.transform);
        boss.transform.LookAt(player.transform);

        InvokeRepeating(nameof(BossAttack), 2f, 3f);

    }

    void BossAttack()
    {
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

        Debug.Log("Player HP: " + playerHealth);

        CheckPlayerDeath();
    }

    void CheckBossDeath()
    {
        if (bossHealth <= 0)
        {
            fightStarted = false;

            CancelInvoke();

            bossAnimator.SetTrigger("Die");

            Debug.Log("Boss Defeated");
        }
    }

    void CheckPlayerDeath()
    {
        if (playerHealth <= 0)
        {
            fightStarted = false;

            CancelInvoke();

            playerAnimator.SetTrigger("Die");

            Debug.Log("Player Died");
        }
    }
}