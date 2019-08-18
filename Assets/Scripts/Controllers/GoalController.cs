using UnityEngine;

/**
 * Updates the score when there is a goal (ball collides with it)
 */
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
