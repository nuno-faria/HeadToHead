using UnityEngine;
using UnityEngine.Events;

/**
 * Checks when the player lands (on) the ground 
 */
public class GroundCheckController : MonoBehaviour {

    public LayerMask ground;
    public UnityEvent onLand;


    //on the ground
    private void OnTriggerEnter2D(Collider2D collision) {
        if (((1 << collision.gameObject.layer) & ground) != 0)
            onLand.Invoke();
    }
}
