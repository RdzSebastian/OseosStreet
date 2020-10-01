using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public float maxSpeed = 4f;
    public float jumpForce = 400f;

    private float currentSpeed;
    private Rigidbody rb;
    public Animator animator;
    private Transform groundCheck;
    private bool onGround;
    private bool isDead = false;
    private bool facingRight = true;
    private bool jump = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        groundCheck = gameObject.transform.Find("GroundCheck");
        currentSpeed = maxSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        onGround = Physics.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));

        if (Input.GetButton("Jump") && onGround)
        {
            jump = true;
            animator.SetBool("IsJumping", true);
        }

        if (!onGround && animator.GetBool("IsJumping"))
        {
            animator.SetBool("IsJumping", false);
        }
    }

    private void FixedUpdate()
    {
        if (!isDead)
        {
            float horizontalMove = Input.GetAxis("Horizontal");
            float verticalMove = Input.GetAxis("Vertical");

            animator.SetFloat("Speed", Math.Abs(horizontalMove) + Math.Abs(verticalMove));


            if (!onGround)
            {
                verticalMove = 0;
            }

            rb.velocity = new Vector3(horizontalMove * currentSpeed, rb.velocity.y, verticalMove * currentSpeed);

            if(horizontalMove > 0 && !facingRight)
            {
                Flip();
            }
            else if (horizontalMove < 0 && facingRight)
            {
                Flip();
            }

            if (jump)
            {
                jump = false;
                rb.AddForce(Vector3.up * jumpForce);
            }
        }

    }

    private void Flip()
    {
        facingRight = !facingRight;

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
