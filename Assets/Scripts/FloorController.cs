using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Used to add particles
public class FloorController : MonoBehaviour {

    public ParticleSystem ps;


    private void OnCollisionEnter2D(Collision2D collision) {
        float m = collision.relativeVelocity.magnitude;
        if (m > 3) {
            ParticleSystem particle = Instantiate(ps);
            particle.transform.position = new Vector2(collision.transform.position.x, -4.2f);

            ParticleSystem.EmissionModule emiss = particle.emission;
            emiss.rateOverTime = Mathf.Min(m * 10, 150);

            ParticleSystem.MainModule main = particle.main;
            main.startSpeed = Mathf.Max(5, Mathf.Min(m / 3, 10));
        }
    }
}
