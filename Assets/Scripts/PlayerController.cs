using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public Rigidbody2D rb;

    //MOVE VARS
    public float moveSpeed = 250f;
    public float sprintSpeed = 500f;
    private float horizontalIntensity = 0f;
    private float sprint = 0f;
    private bool onTheAir = false;

    //JUMP VARS
    private bool jump = false;
    public float jumpForce = 250000f;
    //0 - no jump,  1 - first jump,  2 - second jump
    private int jumpState = 0;
    //check if the the user released the jump button so it can jump again
    private bool jumpRelease = true;

    //KICK VARS
    public float kickForce = 600f;
    private bool kick = false;
    private bool kickRelease = true;
    private float verticalIntensity = 0;
    private Rigidbody2D ball;

    //1 -> facing right, -1 -> facing left
    private int direction = 1;


    void Update() {
        horizontalIntensity = Input.GetAxisRaw("Horizontal");
        sprint = Input.GetAxisRaw("Sprint") + Input.GetAxisRaw("SprintC");

        if (Input.GetAxisRaw("Jump") != 0) {
            if (jumpRelease) {
                int prevJumpState = jumpState;
                jumpState = Mathf.Min(2, jumpState + 1);
                if (jumpState != prevJumpState) {
                    jump = true;
                    jumpRelease = false;
                }
            }
        }
        else {
            jumpRelease = true;
        }

        if (Input.GetAxisRaw("Kick") != 0) {
            if (kickRelease) {
                kick = true;
                kickRelease = false;
            }
        }
        else {
            kickRelease = true;
        }

        verticalIntensity = Input.GetAxisRaw("Vertical");
    }


    void FixedUpdate() {
        UpdateMovement();

        if (kick)
            Kick();

        if ((direction == -1 && horizontalIntensity > 0)
            || (direction == 1) && horizontalIntensity < 0)
            Flip();
    }


    void UpdateMovement() {
        float xVel = 0f;
        if (sprint == 0 || onTheAir)
            xVel = horizontalIntensity * moveSpeed * Time.fixedDeltaTime;
        else
            xVel = horizontalIntensity * sprintSpeed * Time.fixedDeltaTime;

        rb.velocity = new Vector2(xVel, rb.velocity.y);

        if (jump) {
            float yForce = jumpForce * Time.fixedDeltaTime;
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(new Vector2(0, yForce));
            jump = false;
        }
    }


    void Kick() {
        if (ball) {
            Vector2 force = new Vector2(1f, verticalIntensity) * kickForce * direction;
            ball.AddForce(force);
        }
        kick = false;
    }

    
    void Flip() {
        direction = -1 * direction;
        transform.Rotate(0f, 180f, 0f);
    }


    public void OnTheGround() {
        jumpState = 0;
        onTheAir = false;
    }


    public void OffTheGround() {
        onTheAir = true;
    }

    public void BallInRange(Rigidbody2D ball) {
        this.ball = ball;
    }

    public void BallOutOfRange() {
        ball = null;
    }
}
