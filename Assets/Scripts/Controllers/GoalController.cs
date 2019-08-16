using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalController : MonoBehaviour {

    public char player;
    public AudioSource audioSource;

    public void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "ball") {
            audioSource.Play();
            GameManager.gm.NewRound(player);
        }
    }
}
