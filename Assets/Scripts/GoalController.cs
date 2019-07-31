using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalController : MonoBehaviour {

    public char player;

    public void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "ball")
            GameManager.gm.NewRound(player);
    }
}
