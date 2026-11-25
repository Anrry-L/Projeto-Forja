using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D playerRb;
    public float moveSpeed = 5f;
    public float runMultiplier = 1.8f;
    [SerializeField] private float knockbackDuration = 0.2f;
    private bool isKnockedBack = false;
    private bool isRunning = false;
    private Vector2 movementDirection;
    public Vector2 FacingDirection { get; private set; } = Vector2.down;

    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        

        movementDirection = new Vector2(
        Input.GetAxisRaw("Horizontal"),
        Input.GetAxisRaw("Vertical")
        ).normalized;

        isRunning = Input.GetKey(KeyCode.LeftShift);

        if (movementDirection != Vector2.zero)
        {
            FacingDirection = movementDirection;
        }

    }

    void FixedUpdate()
    {
        if (isKnockedBack)
        {
            return;
        }

        float currentSpeed = isRunning ? moveSpeed * runMultiplier : moveSpeed;
        playerRb.linearVelocity = movementDirection * currentSpeed;
    }

    public void ApplyKnockback(Vector2 knockback)
    {
        StartCoroutine(KnockbackRoutine(knockback));
    }

    private IEnumerator KnockbackRoutine(Vector2 knockback)
    {
        isKnockedBack = true;

        for (float timer = 0f; timer < knockbackDuration; timer += Time.fixedDeltaTime)
        {
            float progress = timer / knockbackDuration;
            playerRb.linearVelocity = Vector2.Lerp(knockback, Vector2.zero, progress);
            yield return new WaitForFixedUpdate();
        }

        playerRb.linearVelocity = Vector2.zero;
        isKnockedBack = false;
    }

    private void OnDisable()
    {
        movementDirection = Vector2.zero;
        playerRb.linearVelocity = Vector2.zero;
    }
}