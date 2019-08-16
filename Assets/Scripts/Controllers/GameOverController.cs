using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour {

    public TextMeshProUGUI text;
    public Transform firework;
    public float fireworkIntervalMin = 0.1f;
    public float fireworkIntervalMax = 1f;
    private float fireworkInterval;
    public float timer = 0;
    private float endTime = 1f;
    private float endTimer = 0f;


    void Start() {
        text.text = MainManager.winner + " WON!";
        fireworkInterval = Random.Range(fireworkIntervalMin, fireworkIntervalMax);
    }


    void Update() {
        endTimer += Time.deltaTime;

        if (endTimer >= endTime && Input.anyKeyDown)
            SceneManager.LoadScene("Menu");

        timer += Time.deltaTime;
        if (timer >= fireworkInterval) {
            Transform o = Instantiate(firework);
            o.position = new Vector2(Random.Range(-8f, 8f), Random.Range(-4f, 4f));
            timer = 0;
            fireworkInterval = Random.Range(fireworkIntervalMin, fireworkIntervalMax);
        }
    }
}
