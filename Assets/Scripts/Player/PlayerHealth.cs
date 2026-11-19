using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int life = 5;
    private bool isDead = false;

    public void TakeDamage(int damage)
    {
        // Impede receber dano depois da morte
        if (isDead)
            return;

        life = Mathf.Max(life, 0);

        life -= damage;

        Debug.Log("Player recebeu um hit! Vida atual: " + life);

        if (life <= 0)
        {
            Die();
        }
    }

    
    void Die()
    {
        if (isDead)
        {
            return;
        }

        isDead = true;

        Debug.Log("Player morreu!");

        PlayerController controller = GetComponent<PlayerController>();

        if (controller != null)
        {
            controller.enabled = false;
        }

        SpriteRenderer spriteR = GetComponent<SpriteRenderer>();

        spriteR.flipY = true;

    }
}
