using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

//True conditions -> left branch
//False conditions -> right branch
class DecisionTree {


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
}