using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour {

    //singleton
    public static GameManager gm;

    public GameObject ball;
    public GameObject player1;
    public GameObject player2;
    private Rigidbody2D ballRb;
    private PlayerController player1Controller;
    private PlayerController player2Controller;
    public TextMeshProUGUI scoreText;
    public Color colorP1;
    public Color colorP2;

    private int scoreP1 = 0;
    private int scoreP2 = 0;
    private string colorP1S;
    private string colorP2S;


    void Start() {
        gm = this;
        ballRb = ball.GetComponent<Rigidbody2D>();
        colorP1S = "#" + ColorUtility.ToHtmlStringRGB(colorP1);
        colorP2S = "#" + ColorUtility.ToHtmlStringRGB(colorP2);
        player1Controller = player1.GetComponent<PlayerController>();
        player2Controller = player2.GetComponent<PlayerController>();
        ResetBall();
        UpdateText();
    }


    public void NewRound(char concedingPlayer) {
        if (concedingPlayer == '1')
            scoreP2++;
        else
            scoreP1++;

        UpdateText();
        ResetBall();
        ResetPlayers();
    }


    private void UpdateText() {
        scoreText.text = "<color=" + colorP1S + ">" + scoreP1 +
                         "<color=#cccccc>" + " : " + 
                         "<color=" + colorP2S + ">" + scoreP2;
    }

    private void ResetBall() {
        ballRb.velocity = Vector2.zero;
        ballRb.angularVelocity = 0;
        ball.transform.position = new Vector2(0f, Random.Range(0f, 4f));
    }

    private void ResetPlayers() {
        player1.transform.position = new Vector2(-6.93f, -3.51f);
        player1Controller.ResetTransform();

        player2.transform.position = new Vector2(6.93f, -3.51f);
        player2Controller.ResetTransform();
    }
}
