
using System;
using System.Collections.Generic;

class BasketTree {

    public static DecisionTree CreateTree() {
        DecisionTree MOVE_RIGHT = new DecisionTree("MOVE_RIGHT", (state, data) => data["horizontal"] = 1, null, null, null);

		DecisionTree MOVE_LEFT = new DecisionTree("MOVE_LEFT", (state, data) => data["horizontal"] = -1 * 1, null, null, null);

		// Go to a position previously defined
		DecisionTree MOVE_TO_CUSTOM_POS = new DecisionTree("MOVE_TO_CUSTOM_POS", null, (state, data) => state["selfPosX"] < data["_x"], new List<DecisionTree> {MOVE_RIGHT}, new List<DecisionTree> {MOVE_LEFT});

		// Go to where the ball is expected to fall
		DecisionTree GO_PREDICT_FALL = new DecisionTree("GO_PREDICT_FALL", (state, data) => {data["_y"] = -3.65; data["_m"] = state["ballVelY"] / state["ballVelX"]; data["_b"] = state["ballPosY"] - data["_m"] * state["ballPosX"]; data["_x"] = (data["_y"] - data["_b"]) / data["_m"]; data["_x"] = data["_x"] > 8.5 ? 8.5 - (data["_x"] - 8.5) : data["_x"]; data["_x"] = data["_x"] + 0.25;}, null, new List<DecisionTree> {MOVE_TO_CUSTOM_POS}, null);

		DecisionTree JUMP = new DecisionTree("JUMP", (state, data) => data["jump"] = 1, null, null, null);

		// Is in range to jump?
		DecisionTree JUMP_TO_BALL = new DecisionTree("JUMP_TO_BALL", null, (state, data) => Math.Abs(state["selfPosX"] - state["ballPosX"]) < 1 &&  Math.Abs(state["selfPosY"] - state["ballPosY"]) < 3 && Math.Abs(state["selfPosY"] - state["ballPosY"]) > 0.2, new List<DecisionTree> {JUMP}, null);

		DecisionTree SPRINT = new DecisionTree("SPRINT", (state, data) => data["sprint"] = 1, null, null, null);

		DecisionTree ROOT = new DecisionTree("ROOT", null, null, new List<DecisionTree> {GO_PREDICT_FALL, JUMP_TO_BALL, SPRINT}, null);

        return ROOT;
    }
}
