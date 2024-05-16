using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGridController : MonoBehaviour
{
    public float speed = 5f;
    public Transform movePoint;
    public Animator anim;
    public bool check = false;
    public LayerMask stopMovement;
    void Start()
    {
        movePoint.parent = null; 
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, movePoint.position) <= 0f) // chequea que ya ha llegado a la casilla antes de volver a mover
        {
            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
            {
                if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f), .2f, stopMovement)) // chequea si no hay colision
                {
                    movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
                }
            }
            else if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
            {
                if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f), .2f, stopMovement))
                {
                    movePoint.position += new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f);
                }
            }
            if (Input.GetAxisRaw("Horizontal") < 0 && !check)// Para cuando el player gira a la izquierda, para que flipee el objeto
            {
                transform.Rotate(new Vector3(0, 180, 0));
                check = true;
            }
            if (Input.GetAxisRaw("Horizontal") > 0 && check)
            {
                transform.Rotate(new Vector3(0, 180, 0));
                check = false;
            }
            anim.SetFloat("Horizontal", Input.GetAxisRaw("Horizontal"));
            anim.SetFloat("Vertical", Input.GetAxisRaw("Vertical"));
            if(Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
            {
                anim.SetBool("Moving", false);
            }
            else
            {
                anim.SetBool("Moving", true);
            }
        }
    }
}
