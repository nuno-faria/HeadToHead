
using System;
using System.Collections.Generic;

class VolleyTree {

    public static DecisionTree CreateTree() {
        DecisionTree MOVE_RIGHT = new DecisionTree("MOVE_RIGHT", (state, data) => data["horizontal"] = 1.75, null, null, null);
		DecisionTree MOVE_LEFT = new DecisionTree("MOVE_LEFT", (state, data) => data["horizontal"] = -1 * 1.75, null, null, null);
		DecisionTree MOVE_TO_CUSTOM_POS = new DecisionTree("MOVE_TO_CUSTOM_POS", null, (state, data) => state["selfPosX"] < data["_x"], new List<DecisionTree> {MOVE_RIGHT}, new List<DecisionTree> {MOVE_LEFT});
		DecisionTree FOLLOW_BALL = new DecisionTree("FOLLOW_BALL", (state, data) => data["_x"] = state["ballPosX"], (state, data) => Math.Abs(state["selfPosX"] - state["ballPosX"]) > 0.4, new List<DecisionTree> {MOVE_TO_CUSTOM_POS}, null);
		DecisionTree SPRINT = new DecisionTree("SPRINT", (state, data) => data["sprint"] = 1, null, null, null);
		DecisionTree JUMP = new DecisionTree("JUMP", (state, data) => data["jump"] = 1, null, null, null);
		DecisionTree JUMP_TO_BALL = new DecisionTree("JUMP_TO_BALL", null, (state, data) => Math.Abs(state["selfPosX"] - state["ballPosX"]) < 2 &&  Math.Abs(state["selfPosY"] - state["ballPosY"]) < 2, new List<DecisionTree> {JUMP}, null);
		DecisionTree GO_PREDICT_FALL = new DecisionTree("GO_PREDICT_FALL", (state, data) => {data["_ballFallX"] = (-3.6 - state["ballPosY"]) / (state["ballVelY"] / state["ballVelX"]) + state["ballPosX"]; data["_ballFallX"] = data["_ballFallX"] > 8.9 ? 8.9 - (data["_ballFallX"] - 8.9) : data["_ballFallX"]; data["_x"] = data["_ballFallX"];}, null, new List<DecisionTree> {MOVE_TO_CUSTOM_POS}, null);
		DecisionTree GO_BALL = new DecisionTree("GO_BALL", null, (state, data) => state["ballPosY"] < 0.5 || state["ballVelX"] < 8 || state["ballVelY"] < -4, new List<DecisionTree> {FOLLOW_BALL, SPRINT, JUMP_TO_BALL}, new List<DecisionTree> {GO_PREDICT_FALL, SPRINT, JUMP_TO_BALL});
		DecisionTree GO_WAIT_POS = new DecisionTree("GO_WAIT_POS", (state, data) => data["_x"] = 3, null, new List<DecisionTree> {MOVE_TO_CUSTOM_POS}, null);
		DecisionTree ROOT = new DecisionTree("ROOT", null, (state, data) => state["ballPosX"] > 0, new List<DecisionTree> {GO_BALL}, new List<DecisionTree> {GO_WAIT_POS});
        return ROOT;
    }
}
