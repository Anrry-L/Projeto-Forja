using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int life = 5;
    private int minLife, maxLife;

    private bool isDead = false;
    private bool isInvincible = false;
    [SerializeField] private float blinkTime;


    [SerializeField] private float invincibilityDuration = 2f;


    // 1. Campos: existem enquanto o objeto existir
    private PlayerController controller;
    private SpriteRenderer spriteR;
    private Animator animator;

    // 2. Awake: busca os componentes uma única vez
    private void Awake()
    {
        controller = GetComponent<PlayerController>();
        spriteR = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private IEnumerator InvincibilityRoutine()
    {
            isInvincible = true;

            for (float i = 0; i < invincibilityDuration; i += blinkTime * 2)
            {
                spriteR.enabled = false;
                yield return new WaitForSeconds(blinkTime);
                spriteR.enabled = true;
                yield return new WaitForSeconds(blinkTime);
                Debug.Log(i);
            }

            spriteR.enabled = true;
            isInvincible = false;

    }

    public void TakeDamage(int damage)
    {
        if (isDead || damage <= 0 || isInvincible)
        {
            return;
        }

        life = Mathf.Max(life - damage, minLife);
        Debug.Log("Player recebeu um hit! Vida atual: " + life);


        if (life <= 0)
        {
            Die();
        } else
        {
            StartCoroutine(InvincibilityRoutine());
        }
    }

    public bool HealPlayer(int heal)
    {
        if (isDead || heal <= 0 || life >= maxLife)
        {
            return false;
        }

        life = Mathf.Min(life + heal, maxLife);
        Debug.Log("Player curou " + heal + " de vida | Vida atual: " + life);
        return true;

    }

    private void Die()
    {
        if (isDead)
        {
            return;
        }

        isDead = true;
        Debug.Log("Player morreu!");

        // 3. Usa os campos já preenchidos, sem GetComponent aqui
        if (controller != null)
        {
            controller.enabled = false;
        }

        if (spriteR != null)
        {
            spriteR.flipY = true;
        }
        if (animator != null)
        {
            animator.enabled = false;
        }
    }
}