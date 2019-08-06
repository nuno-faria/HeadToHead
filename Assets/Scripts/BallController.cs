using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour {

    public Rigidbody2D rb;
    //0 = no limit
    public float maxSpeed = 0;


    void Update() {
        if (maxSpeed > 0 && (rb.velocity.x > maxSpeed || rb.velocity.y > maxSpeed)) {
            float x = rb.velocity.x < maxSpeed ? rb.velocity.x : maxSpeed;
            float y = rb.velocity.y < maxSpeed ? rb.velocity.y : maxSpeed;
            rb.velocity = new Vector2(x, y);
        }
    }
}
