using System.Collections;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown = 0.5f;
    [SerializeField] private float knockbackSpeed = 1f;
    private Rigidbody2D enemyRb;
    private bool isAttacking = false;
    [SerializeField] private int attackDamage = 1;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float attackDistance = 0.8f;
    [SerializeField] private Vector2 attackSize = new Vector2(1f, 1f);
    [SerializeField] private float idleTime = 1f;
    [SerializeField] private float attackTime = 0.7f;
    private EnemyController controller;

    void Start()
    {
        controller = GetComponent<EnemyController>();
        enemyRb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (!isAttacking && FindPlayer() != null)
        {
            StartCoroutine(AttackRoutine());
        }
    }

    private IEnumerator AttackRoutine()
    {
        isAttacking = true;
        controller.CanMove = false;

        yield return new WaitForSeconds(attackTime);
        Attack();

        yield return new WaitForSeconds(idleTime);
        controller.CanMove = true;

        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
    }

    private void Attack()
    {
        PlayerHealth target = FindPlayer();

        if (target != null)
        {
            Vector2 direction = ((Vector2)target.transform.position - enemyRb.position).normalized;
            target.TakeDamage(attackDamage, direction * knockbackSpeed);
        }
    }

    private PlayerHealth FindPlayer()
    {
        Collider2D[] hits = Physics2D.OverlapBoxAll(GetAttackCenter(), attackSize, GetAttackAngle(), playerLayer);

        foreach (Collider2D hit in hits)
        {
            if (hit.TryGetComponent(out PlayerHealth playerHealth))
            {
                return playerHealth;
            }
        }

        return null;
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