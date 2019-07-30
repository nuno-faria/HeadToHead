using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalController : MonoBehaviour {

    public void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "ball") {
            Debug.Log("goal");
        }
    }
}
