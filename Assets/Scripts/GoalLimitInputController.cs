using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GoalLimitInputController : MonoBehaviour {

    public TextMeshProUGUI text;


    void Start() {
        text.text = MainManager.goalLimit.ToString() + " ";
    }


    void Update() {
        string t = text.text.Remove(text.text.Length - 1);
        MainManager.goalLimit = string.IsNullOrEmpty(t) ? 0 : int.Parse(t);
    }
}
