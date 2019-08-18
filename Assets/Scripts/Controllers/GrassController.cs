using UnityEngine;

/**
 * Triggers the respective animation when colliding with a player or a ball
 */
public class GrassController : MonoBehaviour {

    public Animator animator;

    public float resetTime = 0.2f;
    private float timer = 0f;


    private void OnTriggerEnter2D(Collider2D collision) {
        //move only when touching the ball or the players feet
        if (collision.GetType() == typeof(CircleCollider2D)) {
            if (collision.attachedRigidbody.velocity.x > 0)
                animator.SetInteger("push_direction", 1);
            else
                animator.SetInteger("push_direction", -1);
            timer = 0f;
        }
    }


    private void Update() {
        timer += Time.fixedDeltaTime;
        if (timer >= resetTime)
            animator.SetInteger("push_direction", 0);
    }
}
