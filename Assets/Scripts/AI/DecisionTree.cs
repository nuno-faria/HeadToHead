using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

//True conditions -> left branch
//False conditions -> right branch
class DecisionTree {

    //public Node Node { get; set; }

    public string Name;

    //Set data
    public Action<Dictionary<string, object>, Dictionary<string, object>> Action { get; set; }

    //Find which branch to follow next
    public Func<Dictionary<string, object>, Dictionary<string, object>, bool> Decision { get; set; }


    //Left
    public List<DecisionTree> Left { get; set; }

    //Right
    public List<DecisionTree> Right { get; set; }

    public DecisionTree(string name, 
                        Action<Dictionary<string, dynamic>, Dictionary<string, dynamic>> action,
                        Func<Dictionary<string, dynamic>, Dictionary<string, dynamic>, bool> decision,
                        List<DecisionTree> left, 
                        List<DecisionTree> right) {
        Name = name;
        Action = action;
        Decision = decision;
        Left = left;
        Right = right;
    }



    //Executes the tree's node and subtrees
    // state -> observation of the environment
    // data -> dictionary for the nodes' actions to store data
    //public void Exec(Dictionary<string, object> state, Dictionary<string, object> data) {
    //    bool result = Node.Exec(state, data);
    //
    //    //Leaf
    //    if (Left == null && Right == null) {
    //        return;
    //    }
    //    else {
    //        if (result)
    //            Left.ForEach(x => x.Exec(state, data));
    //        else
    //            Right.ForEach(x => x.Exec(state, data));
    //    }
    //}

    public void Exec(Dictionary<string, object> state, Dictionary<string, object> data) {
        Action?.Invoke(state, data);
        bool result =  Decision != null ? Decision(state, data) : true;

        //Leaf
        if (Left == null && Right == null) {
            return;
        }
        else {
            if (result)
                Left?.ForEach(x => x.Exec(state, data));
            else
                Right?.ForEach(x => x.Exec(state, data));
        }
    }


    //ex path: "lrrlr"
    /*
    public void Add(Node node, string path) {

        //leaf
        if (path.Length == 0)
            Node = node;

        //add new tree to list (repeated path)
        if (path.Length == 1) {
            if (path[0] == 'l') {
                if (Left == null)
                    Left = new List<DecisionTree>();
                Left.Add(new DecisionTree());
                Left[0].Add(node, path.Remove(0));
            }
            else {
                if (Right == null)
                    Right = new List<DecisionTree>();
                Right.Add(new DecisionTree());
                Right[0].Add(node, path.Remove(0));
            }
        }


        //follow path
        //always adds to the last inserted tree
        if (path.Length > 1) {
            if (path[0] == 'l') {
                Left.Last().Add(node, path.Remove(1));
            }
            else {
                Right.Last().Add(node, path.Remove(1));
                return;
            }
        }
    }
    */
}