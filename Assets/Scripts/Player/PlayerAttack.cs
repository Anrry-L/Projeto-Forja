using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown = 0.5f;
    private float cooldownTimer;
    [SerializeField] private Vector2 attackSize = new Vector2(1f, 1f);
    [SerializeField] private int attackDamage = 1;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float attackDistance = 0.8f;
    private PlayerController controller;

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && cooldownTimer <= 0)
        {
            Attack();
            cooldownTimer = attackCooldown;
        }

        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }
    }

    private void Attack()
    {
        Collider2D[] hits = Physics2D.OverlapBoxAll(GetAttackCenter(), attackSize, GetAttackAngle(), enemyLayer);

        Debug.Log("Player atacou! Inimigos atingidos: " + hits.Length);

        foreach (Collider2D hit in hits)
        {
            if (hit.TryGetComponent(out EnemyHealth enemyHealth))
            {
                enemyHealth.TakeDamage(attackDamage);
            }
        }
    }

    private Vector2 GetFacing()
    {
        return controller != null ? controller.FacingDirection : Vector2.down;
    }

    private Vector2 GetAttackCenter()
    {
        return (Vector2)transform.position + GetFacing() * attackDistance;
    }

    private float GetAttackAngle()
    {
        Vector2 facing = GetFacing();
        return Mathf.Atan2(facing.y, facing.x) * Mathf.Rad2Deg;
    }
    private void OnDrawGizmosSelected()
{
    Gizmos.color = Color.red;
    Gizmos.matrix = Matrix4x4.TRS(GetAttackCenter(), Quaternion.Euler(0, 0, GetAttackAngle()), Vector3.one);
    Gizmos.DrawWireCube(Vector3.zero, attackSize);
}
}
