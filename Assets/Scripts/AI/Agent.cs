using System.Collections.Generic;

/**
 * Provides the state and gets the actions from a decision tree
 */
namespace Assets.Scripts.AI {

    public class Agent {

        private DecisionTree tree;


        public Agent(string gameMode) {
            if (gameMode == "football")
                tree = FootballTree.CreateTree();
            else if (gameMode == "volley")
                tree = VolleyTree.CreateTree();
            else
                tree = BasketTree.CreateTree();
        }


        public void Act(Dictionary<string, dynamic> state, Dictionary<string, dynamic> data) {
            tree.Exec(state, data);
        }
    }
}
