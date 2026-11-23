using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private Transform player;
    private Rigidbody2D enemyRb;
    private Vector2 moveDirection;
    public Vector2 FacingDirection { get; private set; } = Vector2.down;
    public bool CanMove { get; set; } = true;

    private void Awake()
    {
        enemyRb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        if (player == null)
        {
            Debug.LogWarning("EnemyController: Player não foi atribuído no Inspector!");
        }
    }

    private void FixedUpdate()
    {
        if (player == null || !CanMove)
        {
            enemyRb.linearVelocity = Vector2.zero;
            return;
        }

        moveDirection = ((Vector2)player.position - enemyRb.position).normalized;
        enemyRb.linearVelocity = moveDirection * moveSpeed;

        if (moveDirection != Vector2.zero)
        {
            FacingDirection = moveDirection;
        }
    }
}