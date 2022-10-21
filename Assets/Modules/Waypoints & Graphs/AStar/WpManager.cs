using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction { UNI, BI }

[System.Serializable]
public struct Link
{
    public Direction direction;
    public GameObject node1;
    public GameObject node2;
}
public class WpManager : MonoBehaviour
{
    public GameObject[] waypoints;
    public Link[] links;
    public Graph graph = new Graph();
    // Start is called before the first frame update
    void Start()
    {
        if (waypoints.Length > 0)
        {
            foreach (GameObject wp in waypoints)
            {
                graph.AddNode(wp);
            }
            foreach (Link l in links)
            {
                graph.AddEdge(l.node1, l.node2);
                if (l.direction == Direction.BI)
                {
                    graph.AddEdge(l.node2, l.node1);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
