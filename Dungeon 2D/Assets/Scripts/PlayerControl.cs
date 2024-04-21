using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float movSpeed;
    public Rigidbody2D rb;
    private float moveX, moveY;
    private Vector2 moveDirection;
    public Animator animator;
    private bool check = false;

    void Update()
    {
        ProcessInputs();
    }

    void FixedUpdate()
    {
        Move();
    }

    private void ProcessInputs()
    {
        moveX = Input.GetAxisRaw("Horizontal");
        moveY = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector2(moveX, moveY).normalized;
        
        animator.SetFloat("Horizontal", moveX);
        animator.SetFloat("Vertical", moveY);
        animator.SetFloat("Speed", moveDirection.sqrMagnitude);

        if(moveDirection.x < 0 && !check)// Para cuando el player gira a la izquierda, para que flipee el objeto
        {
            transform.Rotate(new Vector3(0, 180, 0));
            check = true;
        }
        if (moveDirection.x > 0 && check)
        {
            transform.Rotate(new Vector3(0, 180, 0));
            check = false;
        }
    }

    private void Move()
    {
        rb.velocity = new Vector2(moveDirection.x * movSpeed, moveDirection.y * movSpeed);
    }
}
