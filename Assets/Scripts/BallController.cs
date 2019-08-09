using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour {

    public Rigidbody2D rb;
    public AudioSource audioSource;
    //0 = no limit
    public float maxSpeed = 0;
    public float maxVolume = 0.1f;

    private void Start() {
        audioSource.volume = 0.05f;
    }

    void OnCollisionEnter2D(Collision2D collision) {
        audioSource.volume = Mathf.Min(collision.relativeVelocity.magnitude / (30f / maxVolume), maxVolume);
        if (audioSource.volume > 0.01) {
            PlayAudioSource();
        }
        audioSource.volume = maxVolume;
    }

    void Update() {
        if (maxSpeed > 0 && (rb.velocity.x > maxSpeed || rb.velocity.y > maxSpeed)) {
            float x = rb.velocity.x < maxSpeed ? rb.velocity.x : maxSpeed;
            float y = rb.velocity.y < maxSpeed ? rb.velocity.y : maxSpeed;
            rb.velocity = new Vector2(x, y);
        }
    }

    public void PlayAudioSource() {
        audioSource.pitch = 1 + Random.Range(-0.2f, 0f);
        audioSource.Play();
    }
}
