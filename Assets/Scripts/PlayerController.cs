using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    public Rigidbody2D rb;
    public Animator animator;
    public string controllerName;

    //MOVE VARS
    public float moveSpeed = 250f;
    public float sprintSpeed = 500f;
    //ratio of the x velocity energy kept in the next update
    public float xVelocityMomentum = 0.8f;
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
    public float kickForce = 1000f;
    private bool kick = false;
    private bool kickRelease = true;
    private float verticalInput = 0;
    private float verticalIntensity = 0;
    //maximum time need for full power kick (seconds)
    public float maxKickHoldTime = 0.75f;
    private float kickHoldTime = 0f;
    public Image kickPowerSlider;

    //LIFT VARS
    private bool lift = false;
    private bool liftRelease = true;
    public float liftForce = 500f;

    //STOP VARS
    public float stopPower = 0.15f;
    private bool stop = false;

    //BALL
    private Rigidbody2D ball;

    //DIRECTION
    //1 -> facing right, -1 -> facing 
    public int defaultDirection = 1;
    private int direction;


    //COLOR
    public Color color;
    public Texture2D swapTex;
    public Shader shader;


    void Start() {
        kickPowerSlider.fillAmount = 0f;
        direction = defaultDirection;
        SetUpMaterial();
    }


    void SetUpMaterial() {
        //secondary color (darker version of 'color')
        float H, S, V;
        Color.RGBToHSV(color, out H, out S, out V);
        Color secondary = Color.HSVToRGB(H, S, V * 0.9f);

        //create new texture with the wanted colors
        Texture2D texture = new Texture2D(256, 1, TextureFormat.RGBA32, false, false);
        texture.filterMode = FilterMode.Point;
        texture.SetPixels(swapTex.GetPixels());
        texture.SetPixel(248, 0, color);
        texture.SetPixel(216, 0, secondary);
        texture.Apply();

        //add texture to shader (new material)
        Material mat = new Material(shader);
        mat.SetTexture("_SwapTex", texture);

        //add material to sprite
        GetComponent<SpriteRenderer>().material = mat;
    }


    void Update() {
        //horizontal
        horizontalIntensity = Input.GetAxisRaw("Horizontal" + controllerName);

        //vertical
        verticalInput = Input.GetAxisRaw("Vertical" + controllerName);

        //sprint
        sprint = Input.GetAxisRaw("Sprint" + controllerName);

        //jump
        if (Input.GetAxisRaw("Jump" + controllerName) != 0) {
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

        //kick
        if (Input.GetAxisRaw("Kick" + controllerName) != 0) {
            kickRelease = false;
            kickHoldTime = Mathf.Min(maxKickHoldTime, 
                                     Mathf.Max(maxKickHoldTime / 5f, kickHoldTime + Time.fixedDeltaTime));
            kickPowerSlider.fillAmount = Mathf.Floor(kickHoldTime / maxKickHoldTime * 5f) / 5f;
        }
        else {
            if (!kickRelease) {
                kick = true;
                kickRelease = true;
            }
        }

        //lift
        if (Input.GetAxisRaw("Lift" + controllerName) != 0) {
            if (liftRelease) {
                lift = true;
                liftRelease = false;
            }
        }
        else {
            liftRelease = true;
        }

        //stop
        stop = Input.GetAxisRaw("Stop" + controllerName) != 0;

        animator.SetFloat("xVelAbs", Mathf.Abs(rb.velocity.x));
        animator.SetFloat("yVel", rb.velocity.y);
    }


    void FixedUpdate() {
        verticalIntensity = verticalIntensity * 0.85f + verticalInput * 0.15f;

        UpdateMovement();

        if (kick)
            Kick();

        if (lift)
            Lift();

        if (stop)
            Stop();

        if ((direction == -1 && horizontalIntensity > 0)
            || (direction == 1) && horizontalIntensity < 0)
            Flip();
    }


    void UpdateMovement() {
        float xVel = 0f;
        if (sprint == 0)
            xVel = horizontalIntensity * moveSpeed * Time.fixedDeltaTime;
        else
            xVel = horizontalIntensity * sprintSpeed * Time.fixedDeltaTime;
        xVel = xVel * (1 - xVelocityMomentum) + rb.velocity.x * xVelocityMomentum;
        rb.velocity = new Vector2(xVel, rb.velocity.y);

 
        if (jump) {
            float yForce = jumpForce * Time.fixedDeltaTime;
            rb.velocity = new Vector2(rb.velocity.x * 0.5f, 0);
            rb.AddForce(new Vector2(0, yForce));
            jump = false;
        }
    }


    void Kick() {
        if (ball) {
            float holdTimeMultipler = kickHoldTime / maxKickHoldTime;
            float power = kickForce * holdTimeMultipler;
            Vector2 force = new Vector2(1f, verticalIntensity) * power * direction;
            ball.AddForce(force);
        }
        kick = false;
        kickHoldTime = 0f;
        kickPowerSlider.fillAmount = 0;
        animator.SetBool("kick", true);
    }


    void Lift() {
        if (ball) {
            Vector2 force = new Vector2(0f, 1f) * liftForce;
            ball.AddForce(force);
        }
        lift = false;
        animator.SetBool("kick", true);
    }


    void Stop() {
        if (ball) {
            float momentum = 1 - stopPower;
            Vector2 newVel = new Vector2(ball.velocity.x * momentum, ball.velocity.y * momentum);
            ball.velocity = newVel;
        }
        stop = false;
    }

    
    void Flip() {
        direction *= -1;
        transform.Rotate(0f, 180f, 0f);
        kickPowerSlider.transform.Rotate(0f, 180f, 0f);
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


    public void ResetTransform() {
        direction = defaultDirection;
        Vector2 rotation = new Vector2(0f, direction == 1 ? 0 : 180);
        transform.eulerAngles = rotation;
        kickPowerSlider.transform.eulerAngles = rotation;
    }


    public void kickEnded() {
        animator.SetBool("kick", false);
    }
}
