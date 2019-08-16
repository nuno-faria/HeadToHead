using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.AI {

    public class Agent {

        private DecisionTree tree;


        public Agent(string gameMode) {
            if (gameMode == "fottball")
                tree = FootballTree.CreateTree();
            else if (gameMode == "volley")
                tree = VolleyTree.CreateTree();
            else
                tree = BasketTree.CreateTree();
        }


        public void Act(Dictionary<string, dynamic> state, Dictionary<string, dynamic> data) {
            tree.Exec(state, data);
        }
        

        /*
        public float[] Act(Dictionary<string, float> state) {
            float h = 0, j = 0, s = 0;

            if (state["ballPosX"] <= -4) {
                if (state["selfPosX"] < 6.9)
                    h = 1;
                else if (state["selfPosX"] > 6.9)
                    h = -1;

                if (Mathf.Abs(state["selfPosX"] - 6.9f) > 0.5)
                    s = 1;
            }

            else {
                if (state["ballPosX"] > state["selfPosX"])
                    h = 1;
                else
                    h = -1;

                if (state["ballPosY"] - state["selfPosY"] < 2 && state["ballPosY"] - state["selfPosY"] > 1)
                    j = 1;
            }

            return new float[] { h, j, s };

        }
        */
    }
}
