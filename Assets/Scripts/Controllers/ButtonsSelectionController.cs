using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonsSelectionController : MonoBehaviour {

    [Serializable]
    public class Row {
        public List<GameObject> buttons;
    }

    public List<Row> buttonCols;
    public GameObject selectedButton;
    private int selectedCol, selectedRow;

    private bool pressingUp = false;
    private bool pressingDown = false;
    private bool pressingLeft = false;
    private bool pressingRight = false;


    void Start() {
        int i = 0;
        buttonCols.ForEach(col => {
            int j = 0;
            col.buttons.ForEach(b => {
                if (b != selectedButton) {
                    var sr = b.GetComponent<SpriteRenderer>();
                    sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.5f);
                }
                else {
                    selectedCol = i;
                    selectedRow = j;
                }
                j++;
            });
            i++;
        });
    }


    private Func<string, float> axis = name => Input.GetAxis(name);

    void Update() {
        //confirm
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown("joystick button 0"))
            selectedButton.GetComponent<ButtonController>().Action();

        float vertical, horizontal;
        vertical = axis("VerticalK1") + axis("VerticalK2") + axis("VerticalC1") + axis("VerticalC2");
        horizontal = axis("HorizontalK1") + axis("HorizontalK2") + axis("HorizontalC1") + axis("HorizontalC2");

        bool up = vertical > 0;
        bool down = vertical < 0;
        bool left = horizontal < 0;
        bool right = horizontal > 0;


        //button up
        if (!pressingUp && up) {
            ChangeSelectedButton(selectedCol, selectedRow - 1);
            pressingUp = true;
        }
        else if (!up)
            pressingUp = false;

        //button down
        if (!pressingDown && down) {
            ChangeSelectedButton(selectedCol, selectedRow + 1);
            pressingDown = true;
        }
        else if (!down)
            pressingDown = false;

        //button left
        if (!pressingLeft && left) {
            ChangeSelectedButton(selectedCol - 1, selectedRow);
            pressingLeft = true;
        }
        else if (!left)
            pressingLeft = false;

        //button right
        if (!pressingRight && right) {
            ChangeSelectedButton(selectedCol + 1, selectedRow);
            pressingRight = true;
        }
        else if (!right)
            pressingRight = false;
    }


    private void ChangeSelectedButton(int col, int row) {
        try {
            var newButton = buttonCols[col].buttons[row];
            var newButtonSR = newButton.GetComponent<SpriteRenderer>();
            var oldButton = selectedButton;
            var oldButtonSR = oldButton.GetComponent<SpriteRenderer>();

            oldButtonSR.color = new Color(oldButtonSR.color.r, oldButtonSR.color.g, oldButtonSR.color.b, 0.5f);
            newButtonSR.color = new Color(newButtonSR.color.r, newButtonSR.color.g, newButtonSR.color.b, 1f);

            selectedButton = newButton;
            selectedCol = col;
            selectedRow = row;
        }
        catch (Exception) { }
    }

}
