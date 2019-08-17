
using System;
using System.Collections.Generic;

class VolleyTree {

    public static DecisionTree CreateTree() {
        DecisionTree MOVE_RIGHT = new DecisionTree("MOVE_RIGHT", (state, data) => data["horizontal"] = 2, null, null, null);

		DecisionTree MOVE_LEFT = new DecisionTree("MOVE_LEFT", (state, data) => data["horizontal"] = -1 * 2, null, null, null);

		// Go to a position previously defined
		DecisionTree MOVE_TO_CUSTOM_POS = new DecisionTree("MOVE_TO_CUSTOM_POS", null, (state, data) => state["selfPosX"] < data["_x"], new List<DecisionTree> {MOVE_RIGHT}, new List<DecisionTree> {MOVE_LEFT});

		// Not close enough to the ball?
		DecisionTree FOLLOW_BALL = new DecisionTree("FOLLOW_BALL", (state, data) => data["_x"] = state["ballPosX"], (state, data) => Math.Abs(state["selfPosX"] - state["ballPosX"]) > 0.4, new List<DecisionTree> {MOVE_TO_CUSTOM_POS}, null);

		DecisionTree SPRINT = new DecisionTree("SPRINT", (state, data) => data["sprint"] = 1, null, null, null);

		DecisionTree JUMP = new DecisionTree("JUMP", (state, data) => data["jump"] = 1, null, null, null);

		// Player in range of ball?
		DecisionTree JUMP_TO_BALL = new DecisionTree("JUMP_TO_BALL", null, (state, data) => Math.Abs(state["selfPosX"] - state["ballPosX"]) < 2 &&  Math.Abs(state["selfPosY"] - state["ballPosY"]) < 2, new List<DecisionTree> {JUMP}, null);

		// Go to where the ball is expected to fall
		DecisionTree GO_PREDICT_FALL = new DecisionTree("GO_PREDICT_FALL", (state, data) => {data["_y"] = -3.65; data["_m"] = state["ballVelY"] / state["ballVelX"]; data["_b"] = state["ballPosY"] - data["_m"] * state["ballPosX"]; data["_x"] = (data["_y"] - data["_b"]) / data["_m"]; data["_x"] = data["_x"] > 8.5 ? 8.5 - (data["_x"] - 8.5) : data["_x"];}, null, new List<DecisionTree> {MOVE_TO_CUSTOM_POS}, null);

		// Ball state is appropriate to follow?
		DecisionTree GO_BALL = new DecisionTree("GO_BALL", null, (state, data) => state["ballPosY"] < 0 || state["ballVelX"] < 6 || state["ballVelY"] < -4, new List<DecisionTree> {FOLLOW_BALL, SPRINT, JUMP_TO_BALL}, new List<DecisionTree> {GO_PREDICT_FALL, SPRINT, JUMP_TO_BALL});

		// Go to the wait position
		DecisionTree GO_WAIT_POS = new DecisionTree("GO_WAIT_POS", (state, data) => data["_x"] = 3, null, new List<DecisionTree> {MOVE_TO_CUSTOM_POS}, null);

		// Ball in player's area?
		DecisionTree ROOT = new DecisionTree("ROOT", null, (state, data) => state["ballPosX"] > 0, new List<DecisionTree> {GO_BALL}, new List<DecisionTree> {GO_WAIT_POS});

        return ROOT;
    }
}
