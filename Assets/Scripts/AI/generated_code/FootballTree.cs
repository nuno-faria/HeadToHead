
using System;
using System.Collections.Generic;

class FootballTree {

    public static DecisionTree CreateTree() {
        DecisionTree MOVE_RIGHT = new DecisionTree("MOVE_RIGHT", (state, data) => data["horizontal"] = 1, null, null, null);

		DecisionTree MOVE_LEFT = new DecisionTree("MOVE_LEFT", (state, data) => data["horizontal"] = -1 * 1, null, null, null);

		// Go to a position previously defined
		DecisionTree MOVE_TO_CUSTOM_POS = new DecisionTree("MOVE_TO_CUSTOM_POS", null, (state, data) => state["selfPosX"] < data["_x"], new List<DecisionTree> {MOVE_RIGHT}, new List<DecisionTree> {MOVE_LEFT});

		// Move to self area
		DecisionTree CAUTIOUS = new DecisionTree("CAUTIOUS", (state, data) => data["_x"] = 6.7, null, new List<DecisionTree> {MOVE_TO_CUSTOM_POS}, null);

		DecisionTree JUMP = new DecisionTree("JUMP", (state, data) => data["jump"] = 1, null, null, null);

		// Low ball and player behind it?
		DecisionTree GET_OVER_BALL = new DecisionTree("GET_OVER_BALL", (state, data) => data["_x"] = 6.7, (state, data) => Math.Abs(state["ballPosX"] - state["selfPosX"]) < 1 && Math.Abs(state["ballVelY"]) < 4, new List<DecisionTree> {JUMP, MOVE_TO_CUSTOM_POS}, new List<DecisionTree> {MOVE_TO_CUSTOM_POS});

		// Move to ball. Ball in range to push it?
		DecisionTree ATTACK = new DecisionTree("ATTACK", (state, data) => data["_x"] = state["ballPosX"] + 0.25, (state, data) => Math.Abs(state["selfPosX"] - state["ballPosX"]) < 1 && Math.Abs(state["selfPosY"] - state["ballPosY"]) < 1 && Math.Abs(state["selfPosY"] - state["ballPosY"]) > 0.5, new List<DecisionTree> {MOVE_TO_CUSTOM_POS, JUMP}, new List<DecisionTree> {MOVE_TO_CUSTOM_POS});

		// Ball going to self area and player behind the ball
		DecisionTree REACT = new DecisionTree("REACT", null, (state, data) => state["ballVelX"] > 0 && state["selfPosX"] < state["ballPosX"] + 0.2, new List<DecisionTree> {GET_OVER_BALL}, new List<DecisionTree> {ATTACK});

		DecisionTree SPRINT = new DecisionTree("SPRINT", (state, data) => data["sprint"] = 1, null, null, null);

		// Ball is in the other player's area?
		DecisionTree ROOT = new DecisionTree("ROOT", null, (state, data) => state["ballPosX"] < -6, new List<DecisionTree> {CAUTIOUS}, new List<DecisionTree> {REACT, SPRINT});

        return ROOT;
    }
}
