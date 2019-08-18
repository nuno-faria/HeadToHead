using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * Controls a game (positon of the ball and players, score, ...) of football, basket or volley
 */
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
    public string gameType;
    public GameObject pauseMenu;

    private int scoreP1 = 0;
    private int scoreP2 = 0;
    private string colorP1S;
    private string colorP2S;

    private bool paused = false;
    private GameObject pauseMenuObject;

    //volley only (to control max time on each area)
    [Header("Volley Specific Vars")]
    public float maxTimeInArea = 7f; //seconds, 0 -> no limit
    public TextMeshProUGUI volleyTimerText;
    private char lastArea = '0';
    private float areaTimer = 0;

    void Start() {
        gm = this;
        ballRb = ball.GetComponent<Rigidbody2D>();
        colorP1S = "#" + ColorUtility.ToHtmlStringRGB(colorP1);
        colorP2S = "#" + ColorUtility.ToHtmlStringRGB(colorP2);
        player1Controller = player1.GetComponent<PlayerController>();
        player2Controller = player2.GetComponent<PlayerController>();

        if (!MainManager.twoPlayers)
            SetupAI();

        ResetBall();
        UpdateText();
    }


    void SetupAI() {
        player2Controller.cpu = true;
        player2Controller.otherPlayer = player1Controller.gameObject;
        player2Controller.gameBall = ballRb;
        player2Controller.gameMode = gameType;
    }


    void Update() {
        //volley game timer
        if (gameType == "volley" && maxTimeInArea > 0)
            UpdateVolleyTimer();

        //pause
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown("joystick button 7"))
            PauseHandler();
    }


    private void UpdateVolleyTimer() {
        areaTimer += Time.deltaTime;
        volleyTimerText.SetText(Mathf.CeilToInt(maxTimeInArea - areaTimer).ToString());

        //round over
        if (areaTimer >= maxTimeInArea) {
            NewRound(lastArea);
        }
        else {
            char currentArea = ball.transform.position.x < 0 ? '1' : '2';
            //area change, reset timer
            if (currentArea != lastArea)
                areaTimer = 0;
            lastArea = currentArea;
        }
    }


    public void NewRound(char concedingPlayer) {
        if (concedingPlayer == '1')
            scoreP2++;
        else
            scoreP1++;

        CheckGameOver();

        if (gameType == "volley" && maxTimeInArea > 0) {
            areaTimer = 0;
            lastArea = '0';
        }

        UpdateText();
        ResetBall();
        ResetPlayers();
    }


    private void UpdateText() {
        scoreText.text = "<color=" + colorP1S + ">" + scoreP1 +
                         "<color=#cccccc>" + " : " + 
                         "<color=" + colorP2S + ">" + scoreP2;
    }


    private void CheckGameOver() {
        if (MainManager.goalLimit > 0) {
            if (scoreP1 == MainManager.goalLimit) {
                MainManager.winner = "PLAYER 1";
                SceneManager.LoadScene("GameOver");
            }

            if (scoreP2 == MainManager.goalLimit) {
                MainManager.winner = "PLAYER 2";
                SceneManager.LoadScene("GameOver");
            }
        }
    }


    private void ResetBall() {
        if (gameType != "volley") {
            ballRb.velocity = Vector2.zero;
        }
        else {
            float direction = Random.Range(-1f, 1f) <= 0 ? -1 : 1;
            ballRb.velocity = new Vector2(direction * 4f, Random.Range(2f, 4f));
        }

        ballRb.angularVelocity = 0;
        ball.transform.position = new Vector2(0f, Random.Range(0f, 4f));
    }


    private void ResetPlayers() {
        player1.transform.position = new Vector2(-6.93f, -3.54f);
        player1Controller.ResetTransform();

        player2.transform.position = new Vector2(6.93f, -3.54f);
        player2Controller.ResetTransform();
    }


    private void PauseHandler() {
        if (!paused) {
            paused = true;
            Time.timeScale = 0;
            AudioListener.pause = true;
            pauseMenuObject = Instantiate(pauseMenu);
        }
        else {
            paused = false;
            Destroy(pauseMenuObject);
            Time.timeScale = 1;
            AudioListener.pause = false;
        }
    }
}
