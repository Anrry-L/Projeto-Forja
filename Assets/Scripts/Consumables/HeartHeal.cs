using UnityEngine;

public class HeartHeal : MonoBehaviour
{
    private int heal = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.TryGetComponent(out PlayerHealth playerHealth))
        {
            if (playerHealth.HealPlayer(heal))
            {
                Destroy(gameObject);
            }
        }
    }
}