using UnityEngine;
using UnityEngine.Events;

/**
 * Checks to see if the ball is in range of the player (for stops, lifts and kicks)
 */ 
public class BallCheckController : MonoBehaviour {

    [System.Serializable]
    public class Rigidbody2DEvent : UnityEvent<Rigidbody2D> { }

    [SerializeField]
    public Rigidbody2DEvent onBallEnter;
    public UnityEvent onBallExit;


    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "ball")
            onBallEnter.Invoke(collision.attachedRigidbody);
    }


    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag == "ball")
            onBallExit.Invoke();
    }
}
