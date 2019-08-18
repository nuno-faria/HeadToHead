using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/**
 * Controls the menus navigation and stores the inputs for each player
 */
public class MainManager : MonoBehaviour {

    public static MainManager mm;
    public AudioSource audioSource;
    public static bool twoPlayers;
    public static int goalLimit = 5;
    public static Dictionary<string, string> inputs = 
        new Dictionary<string, string> {
            {  "P1", "K1" },
            {  "P2", "K2" }
        };
    public static string winner;
    public static bool created = false;



    void Start() {
        //used so only one instance of the object exists
        if (!created) {
            DontDestroyOnLoad(this);
            created = true;
            mm = this;
        }
        else {
            Destroy(this.gameObject);
        }
    }


    public void LoadMainMenu() {
        Time.timeScale = 1;
        AudioListener.pause = false;
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }


    public void LoadModePicker1P() {
        twoPlayers = false;
        SceneManager.LoadScene("ModePicker", LoadSceneMode.Single);
    }


    public void LoadModePicker2P() {
        twoPlayers = true;
        SceneManager.LoadScene("ModePicker", LoadSceneMode.Single);
    }


    public void LoadFootball() {
        SceneManager.LoadScene("Football", LoadSceneMode.Single);
    }


    public void LoadBasket() {
        SceneManager.LoadScene("Basket", LoadSceneMode.Single);
    }


    public void LoadVolley() {
        SceneManager.LoadScene("Volley", LoadSceneMode.Single);
    }


    public void LoadControls() {
        SceneManager.LoadScene("Controls", LoadSceneMode.Single);
    }


    public void PlaySound(AudioClip clip) {
        audioSource.clip = clip;
        audioSource.Play();
    }
}
