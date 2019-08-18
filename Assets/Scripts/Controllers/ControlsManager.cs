using System.Collections.Generic;
using TMPro;
using UnityEngine;

/**
 * Controls the Controls Menu (changing controls)
 */
public class ControlsManager : MonoBehaviour {

    public TextMeshProUGUI p1ChoiceText;
    public TextMeshProUGUI p2ChoiceText;
    private string p1Choice;
    private string p2Choice;
    private List<string> inputs;


    private void Start() {
        p1Choice = MainManager.inputs["P1"];
        p2Choice = MainManager.inputs["P2"];
        p1ChoiceText.text = p1Choice;
        p2ChoiceText.text = p2Choice;
    }


    void Update() {
        inputs = GetInputs();
        if (!inputs.Contains(p1Choice)) {
            if (p2Choice == "K1")
                p1Choice = "K2";
            else
                p1Choice = "K1";
            p1ChoiceText.text = p1Choice;
        }
        if (!inputs.Contains(p2Choice)) {
            if (p1Choice == "K1")
                p2Choice = "K2";
            else
                p2Choice = "K1";
            p2ChoiceText.text = p2Choice;
        }
    }


    private List<string> GetInputs() {
        List<string> inputs = new List<string>{ "K1", "K2" };
        string[] joysticks = Input.GetJoystickNames();
        int j = 1;
        for (int i=0; i<joysticks.Length && j<=4; i++) {
            if (joysticks[i] != "") {
                inputs.Add("C" + j);
                j++;
            }
        }
        return inputs;
    }


    private void ChangeControls(string player, ref string playerChoice, ref TextMeshProUGUI playerText, int direction) {
        int idx = inputs.IndexOf(playerChoice);
        idx = (idx + direction) % inputs.Count;
        idx = idx >= 0 ? idx : inputs.Count - 1;
        if ((p1Choice == inputs[idx] || p2Choice == inputs[idx]) && !(playerChoice == inputs[idx])) {
            idx = (idx + direction) % inputs.Count;
            idx = idx >= 0 ? idx : inputs.Count - 1;
        }

        playerChoice = inputs[idx];
        playerText.text = playerChoice;
        MainManager.inputs[player] = playerChoice;
    }


    public void ChangeControlsP1Left() {
        ChangeControls("P1", ref p1Choice, ref p1ChoiceText, -1);
    }


    public void ChangeControlsP1Right() {
        ChangeControls("P1", ref p1Choice, ref p1ChoiceText, +1);
    }


    public void ChangeControlsP2Left() {
        ChangeControls("P2", ref p2Choice, ref p2ChoiceText, -1);
    }


    public void ChangeControlsP2Right() {
        ChangeControls("P2", ref p2Choice, ref p2ChoiceText, +1);
    }
}
