using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int life = 5;
    [SerializeField] private int minLife, maxLife;
    [SerializeField] private float blinkTime;
    [SerializeField] private float invincibilityDuration = 2f;

    private bool isDead = false;
    private bool isInvincible = false;

    // 1. Campos: existem enquanto o objeto existir
    private PlayerController controller;
    private SpriteRenderer spriteR;
    private Animator animator;
    private PlayerAttack playerAttack;
    [SerializeField] private GameManager gameManager;

    // 2. Awake: busca os componentes uma única vez
    private void Awake()
    { 
        playerAttack = GetComponent<PlayerAttack>();
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
        }

        isInvincible = false;
    }

    public void TakeDamage(int damage, Vector2 knockback)
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
        }
        else
        {
            controller.ApplyKnockback(knockback);
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

    public void Die()
    {
        if (isDead)
        {
            return;
        }

        isDead = true;
        Debug.Log("Player morreu!");

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
        if (playerAttack != null)
        {
            playerAttack.enabled = false;
        }
        if (gameManager != null)
        {
            gameManager.RestartGameAfterDead();
        }
        else
        {
            Debug.LogWarning("PlayerHealth: GameManager não foi atribuído no Inspector!");
        }
    }
}