using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GroundCheckController : MonoBehaviour {

    public LayerMask ground;
    public UnityEvent onLand;
    public UnityEvent onOffTheGround;


    private void OnTriggerEnter2D(Collider2D collision) {
        if (((1 << collision.gameObject.layer) & ground) != 0)
            onLand.Invoke();
    }


    private void OnTriggerExit2D(Collider2D collision) {
        if (((1 << collision.gameObject.layer) & ground) != 0)
            onOffTheGround.Invoke();
    }
}
