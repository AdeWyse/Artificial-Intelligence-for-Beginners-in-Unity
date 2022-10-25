using System.Collections.Generic;
using UnityEngine;

public class NodePlanner
{
    public NodePlanner parent;
    public float cost;
    public Dictionary<string, int> state;
    public GAction action;

    public NodePlanner(NodePlanner parent, float cost, Dictionary<string, int> allstates, GAction action)
    {
        this.parent = parent;
        this.cost = cost;
        this.state = new Dictionary<string, int>(allstates);
        this.action = action;
    }

    public NodePlanner(NodePlanner parent, float cost, Dictionary<string, int> allstates, Dictionary<string, int> beliefStates,  GAction action)
    {
        this.parent = parent;
        this.cost = cost;
        this.state = new Dictionary<string, int>(allstates);
        foreach(KeyValuePair<string, int> b in beliefStates)
        {
            if (!this.state.ContainsKey(b.Key))
            {
                this.state.Add(b.Key, b.Value);
            }

        }
        this.action = action;
    }
}
public class GPlanner
{
    public Queue<GAction> Plan(List<GAction> actions, Dictionary<string, int> goal, WorldStates beliefStates)
    {
        List<GAction> usableAction = new List<GAction>();
        foreach (GAction action in actions)
        {
            if (action.IsAchievable())
            {
                usableAction.Add(action);
            }
        }

        List<NodePlanner> leaves = new List<NodePlanner>();
        NodePlanner start = new NodePlanner(null, 0, GWorld.Instance.GetWorld().GetStates(), beliefStates.GetStates(), null);

        bool succes = BuildGraph(start, leaves, usableAction, goal);

        if (!succes)
        {
            return null;
        }

        NodePlanner cheapest = null;

        foreach (NodePlanner leaf in leaves)
        {
            if (cheapest == null)
            {
                cheapest = leaf;
            }
            else
            {
                if (leaf.cost < cheapest.cost)
                {
                    cheapest = leaf;
                }
            }
        }

        List<GAction> result = new List<GAction>();
        NodePlanner n = cheapest;

        while (n != null)
        {
            if (n.action != null)
            {
                result.Insert(0, n.action);
            }
            n = n.parent;
        }

        Queue<GAction> queue = new Queue<GAction>();
        foreach (GAction action in result)
        {
            queue.Enqueue(action);
        }

        return queue;

    }

    private bool BuildGraph(NodePlanner parent, List<NodePlanner> leaves, List<GAction> usableActions, Dictionary<string, int> goal)
    {
        bool foundPath = false;
        foreach (GAction action in usableActions)
        {
            if (action.IsAchievableGiven(parent.state))
            {
                Dictionary<string, int> currentState = new Dictionary<string, int>(parent.state);
                foreach (KeyValuePair<string, int> pair in action.effects)
                {
                    if (!currentState.ContainsKey(pair.Key))
                    {
                        currentState.Add(pair.Key, pair.Value);
                    }
                }

                NodePlanner node = new NodePlanner(parent, parent.cost + action.cost, currentState, action);

                if (GoalAchieved(goal, currentState))
                {
                    leaves.Add(node);
                    foundPath = true;
                }
                else
                {
                    List<GAction> subset = ActionSubset(usableActions, action);
                    bool found = BuildGraph(node, leaves, subset, goal);
                    if (found)
                    {
                        foundPath = true;
                    }

                }
            }

        }
        return foundPath;
    }

    private bool GoalAchieved(Dictionary<string, int> goal, Dictionary<string, int> state)
    {
        foreach (KeyValuePair<string, int> g in goal)
        {
            if (!state.ContainsKey(g.Key))
            {
                return false;
            }
        }
        return true;
    }

    private List<GAction> ActionSubset(List<GAction> action, GAction removeMe)
    {
        List<GAction> subset = new List<GAction>();
        foreach (GAction g in action)
        {
            if (!g.Equals(removeMe))
            {
                subset.Add(g);
            }
        }
        return subset;
    }
}
