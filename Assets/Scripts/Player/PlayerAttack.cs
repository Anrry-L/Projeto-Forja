using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private float atkCooldown;
    public float startAtkCooldown;

    void Start()
    {
        
    }

    void Update()
    {
        if (atkCooldown <= 0)
        {
            atkCooldown = startAtkCooldown;
        } else
        {
            atkCooldown -= Time.deltaTime;
        }
    }
}
