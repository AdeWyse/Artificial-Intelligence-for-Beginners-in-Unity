using System.Collections.Generic;
using UnityEngine;

public class NodeBehaviour 
{
    public enum Status { SUCCESS, RUNNING, FAILURE }
    public Status status;
    public List<NodeBehaviour> children = new List<NodeBehaviour>();
    public int currentChild = 0;
    public string name = "";

    public NodeBehaviour()
    {

    }

    public NodeBehaviour(string n)
    {
        name = n;
    }

    public virtual Status Process()
    {
        return children[currentChild].Process();
    }

    public void AddChild(NodeBehaviour child)
    {
        children.Add(child);
    }

}
