using System;
using TMPro;
using UnityEngine;

/**
 * Controls the goal limit input field
 */
public class GoalLimitInputController : MonoBehaviour {

    public TextMeshProUGUI text;
    private bool pressingUp = false;
    private bool pressingDown = false;


    void Start() {
        text.text = MainManager.goalLimit.ToString() + " ";
    }


    private Func<string, float> axis = name => Input.GetAxis(name);

    void Update() {
        string t = text.text.Remove(text.text.Length - 1);
        int limit = string.IsNullOrEmpty(t) ? 0 : int.Parse(t);

        float vertical = axis("VerticalK1") + axis("VerticalK2") + axis("VerticalC1") + axis("VerticalC2");
        bool up = vertical > 0;
        bool down = vertical < 0;

        if (!pressingUp && up) {
            limit++;
            pressingUp = true;
        }
        else if (!up)
            pressingUp = false;


        if (!pressingDown && down) {
            limit--;
            pressingDown = true;
        }
        else if (!down)
            pressingDown = false;

        limit = limit < 0 ? 0 : limit;

        MainManager.goalLimit = limit;
        //text mesh pro bug - must be updated twice
        text.text = limit + " ";
        text.text = limit + " ";
    }
}
