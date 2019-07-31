using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour {

    //singleton
    public static GameManager gm;

    public GameObject ball;
    private Rigidbody2D ballRb;
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
}
