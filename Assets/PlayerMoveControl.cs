using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveControls : MonoBehaviour
{
    public float speed = 5f;
    private GatherInput gatherInput;
    private Rigidbody2D rigidbody2D;
    private Animator animator;

    private int direction = 1; // to right-hand side

    public float jumpForce;
    public float rayLength;
    public LayerMask groundLayer;
    public Transform leftPoint;

    private bool grounded = false;
    private int jumpCount = 0;
    private int maxJumps = 2;  // จำนวนครั้งที่กระโดดได้สูงสุด

    void Start()
    {
        gatherInput = GetComponent<GatherInput>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        SetAnimatorValues();
        Move();
        CheckStatus();

        float move = Input.GetAxis("Horizontal");
        rigidbody2D.velocity = new Vector2(move * speed, rigidbody2D.velocity.y);

        if (move != 0)
        {
            animator.SetBool("isRunning", true);
            float scaleX = Mathf.Abs(transform.localScale.x) * (move > 0 ? 1 : -1);
            transform.localScale = new Vector3(scaleX, transform.localScale.y, transform.localScale.z);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }

        if (Input.GetButtonDown("Jump") && jumpCount < maxJumps)
        {
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, jumpForce);
            jumpCount++;
            animator.SetBool("isJumping", true);
        }

        animator.SetBool("isJumping", !grounded);
    }

    private void FixedUpdate()
    {
        JumpPlayer();
        Flip();
    }

    private void Move()
    {
        rigidbody2D.velocity = new Vector2(speed * gatherInput.valueX, rigidbody2D.velocity.y);
    }

    private void Flip()
    {
        if (gatherInput.valueX * direction < 0)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            direction *= -1;
        }
    }

    private void SetAnimatorValues()
    {
        animator.SetFloat("Speed", Mathf.Abs(rigidbody2D.velocity.x));
        animator.SetFloat("vSpeed", rigidbody2D.velocity.y);
        animator.SetBool("Grounded", grounded);
    }

    private void JumpPlayer()
    {
        if (gatherInput.jumpInput && jumpCount < maxJumps)
        {
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, jumpForce);
            jumpCount++;
            gatherInput.jumpInput = false;
        }
    }

    private void CheckStatus()
    {
        RaycastHit2D leftCheckHit = Physics2D.Raycast(leftPoint.position, Vector2.down, rayLength, groundLayer);
        grounded = leftCheckHit.collider != null;

        if (grounded)
        {
            jumpCount = 0;  // รีเซ็ตการนับจำนวนครั้งที่กระโดดเมื่อแตะพื้น
        }
    }
}

