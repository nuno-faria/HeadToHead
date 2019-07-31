using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
