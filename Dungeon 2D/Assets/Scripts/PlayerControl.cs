using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float movSpeed;
    public Rigidbody2D rb;
    float moveX, moveY;
    private Vector2 moveDirection;

    void Update()
    {
        ProcessInputs();
    }

    void FixedUpdate()
    {
        Move();
    }

    void ProcessInputs()
    {
        moveX = Input.GetAxisRaw("Horizontal");
        moveY = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector2(moveX, moveY).normalized;   
    }

    void Move()
    {
        rb.velocity = new Vector2(moveDirection.x * movSpeed, moveDirection.y * movSpeed);
    }
}
