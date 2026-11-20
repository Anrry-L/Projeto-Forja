using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D playerRb;
    public float moveSpeed = 5f;
    public float runMultiplier = 1.8f;
    private bool isRunning = false;
    private Vector2 movementDirection;

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

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
}

    void FixedUpdate()
    {

        float currentSpeed = isRunning ? moveSpeed * runMultiplier : moveSpeed;
        playerRb.linearVelocity = movementDirection * currentSpeed;

    }

    private void OnDisable()
    {
        movementDirection = Vector2.zero;
        playerRb.linearVelocity = Vector2.zero;
    }
}