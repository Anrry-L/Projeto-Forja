using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private int life = 5;
    public void TakeDamage(int damage)
    {
        life -= damage;

        life = Mathf.Max(life, 0);

        Debug.Log("Inimigo recebeu dano! Vida: " + life);

        if (life <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Inimigo morreu!");

        Destroy(gameObject);
    }
}