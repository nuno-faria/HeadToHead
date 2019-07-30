using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public Rigidbody2D rb;
    public Transform groundCheck;
    public LayerMask ground;
    public float moveSpeed = 200f;
    public float sprintSpeed = 400f;
    public float jumpForce = 500f;

    private float horizontalMove = 0f;
    private float sprint = 0f;
    private bool jump = false;
    //0 - no jump,  1 - first jump,  2 - second jump
    private int jumpState = 0;
    //check if the the user released the jump button so it can jump again
    private bool jumpRelease = true;
    private bool onTheGround = true;
    private float playerWidth;


    void Start() {
        playerWidth = GetComponent<SpriteRenderer>().bounds.size.x;
    }


    void Update() {
        horizontalMove = Input.GetAxisRaw("Horizontal");
        sprint = Input.GetAxisRaw("Sprint") + Input.GetAxisRaw("SprintC");

        if (Input.GetAxisRaw("Jump") != 0) {
            if (jumpRelease) {
                int prevJumpState = jumpState;
                jumpState = Mathf.Min(2, jumpState + 1);
                if (jumpState != prevJumpState) {
                    jump = true;
                    jumpRelease = false;
                }
                else {
                    jump = false;
                }
            }
            else {
                jump = false;
            }
        }
        else {
            jump = false;
            jumpRelease = true;
        }
    }


    void FixedUpdate() {
        updateMovement();
        updateOnTheGround();
    }


    void updateOnTheGround() {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(groundCheck.position, new Vector2(playerWidth / 4f, 0f), 0, ground);
        onTheGround = false;
        foreach (Collider2D c in colliders)
            if (c.gameObject != gameObject)
                onTheGround = true;

        if (onTheGround)
            jumpState = 0;
    }


    void updateMovement() {
        float xVel = 0f;
        if (sprint == 0)
            xVel = horizontalMove * moveSpeed * Time.fixedDeltaTime;
        else
            xVel = horizontalMove * sprintSpeed * Time.fixedDeltaTime;

        rb.velocity = new Vector2(xVel, rb.velocity.y);

        if (jump) {
            float yForce = jumpForce * Time.fixedDeltaTime;
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(new Vector2(0, yForce));
        }
    }
}
