using System;
using System.Collections.Generic;

/**
 * Decision tree node (action and decision)
 */
class Node {

    //Set data
    public Action<Dictionary<string, object>, Dictionary<string, object>> Action { get; set; }
        
    //Find which branch to follow next
    public Func<Dictionary<string, object>, Dictionary<string, object>, bool> Decision { get; set; }

    public Node(Action<Dictionary<string, object>, Dictionary<string, object>> action,
                Func<Dictionary<string, object>, Dictionary<string, object>, bool> decision) {
        Action = action;
        Decision = decision;
    }

    public bool Exec(Dictionary<string, object> state, Dictionary<string, object> data) {
        Action?.Invoke(state, data);
        return Decision != null ? Decision(state, data) : true;
    }
}
