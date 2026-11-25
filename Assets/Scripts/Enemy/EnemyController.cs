using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private Transform player;
    [SerializeField] private LayerMask wallLayer;
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
        if (player == null || !CanMove || !CanSeePlayer())
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
    private bool CanSeePlayer()
    {
        RaycastHit2D linhaVisao = Physics2D.Linecast(transform.position, player.transform.position, wallLayer);
        Debug.DrawLine(transform.position, player.position, Color.red);
        return linhaVisao.collider == null;
    }
}