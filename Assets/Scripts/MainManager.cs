using UnityEngine;
using UnityEngine.SceneManagement;


public class MainManager : MonoBehaviour {

    public static bool twoPlayers;
    //TODO controls vars

    public void LoadMainMenu() {
        Time.timeScale = 1;
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
}
