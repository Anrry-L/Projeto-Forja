using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D playerRb;
    public int life;
    public float moveSpeed = 5f;
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

    }

    void FixedUpdate()
    {
        playerRb.MovePosition(
            playerRb.position + 
            movementDirection * moveSpeed * Time.fixedDeltaTime);    
    }
}
