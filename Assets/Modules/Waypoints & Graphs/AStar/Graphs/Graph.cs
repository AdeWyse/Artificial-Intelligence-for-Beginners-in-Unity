using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Graph
{
    List<Edge> edges = new List<Edge>();
    List<Node> nodes = new List<Node>();
    public List<Node> pathList = new List<Node>();

    public Graph()
    {

    }

    public void AddNode( GameObject id)
    {
        Node node = new Node(id);
        nodes.Add(node);
    }

    public void AddEdge(GameObject fromNode, GameObject toNode)
    {
        Node from = FindNode(fromNode);
        Node to = FindNode(toNode);
        if( from != null && to != null)
        {
            Edge edge = new Edge(from, to);
            edges.Add(edge);
            from.edgeList.Add(edge);

        }
        
    }

    public Node FindNode(GameObject find)
    {
        foreach (Node node in nodes)
        {
            if(node.getid() == find)
            {
                return node;
            }
        }
        return null;
    }


    public bool AStar(GameObject startId, GameObject endId)
    {
        Node start = FindNode(startId);
        Node end = FindNode(endId);

        if(startId == endId)
        {
            pathList.Clear();
            return false;
        }

        if(start == null || end == null)
        {
            return false;
        }

        List<Node> open = new List<Node>();
        List<Node> closed = new List<Node>();

        float tentative_g_score = 0;

        bool tentativeIsBetter;
        start.g = 0;
        start.h = distance(start, end);
        start.f = start.h;

        open.Add(start);

        while(open.Count > 0)
        {
            int i = lowestF(open);
            Node thisNode = open[i];
            if(thisNode.getid() == end.getid())
            {
                ReconstructPath(start, end);
                return true;
            }

            open.RemoveAt(i);
            closed.Add(thisNode);

            Node neighbour;
            foreach(Edge e in thisNode.edgeList)
            {
                neighbour = e .endNode;

                if(closed.IndexOf(neighbour) > -1)
                {
                    continue;
                }

                tentative_g_score = thisNode.g + distance(thisNode, neighbour);

                if(open.IndexOf(neighbour) == -1)
                {
                    open.Add(neighbour);
                    tentativeIsBetter = true;
                }
                else if (tentative_g_score < neighbour.g)
                {
                    tentativeIsBetter = true;
                }
                else
                {
                    tentativeIsBetter= false;
                }

                if (tentativeIsBetter)
                {
                    neighbour.cameFrom = thisNode;
                    neighbour.g = tentative_g_score;
                    neighbour.h = distance(thisNode, end);
                    neighbour.f = neighbour.g + neighbour.h;
                }
            }
        }
        return false;

    }

    public void ReconstructPath(Node start, Node end)
    {
        pathList.Clear();
        pathList.Add(end);
        var p = end.cameFrom;

        while(p != start && p != null)
        {
            pathList.Insert(0,p);
            p = p.cameFrom;
        }
        pathList.Insert(0, start);
    }

    float distance(Node a, Node b)
    {
        return (Vector3.SqrMagnitude(a.getid().transform.position - b.getid().transform.position));
    }

    int lowestF(List<Node> l)
    {
        float lowestF = 0;
        int count = 0;
        int iteratorCount = 0;

        lowestF = l[0].f;

        for (int i = 0; i < l.Count; i++)
        {
            if (l[i].f < lowestF)
            {
                lowestF = l[i].f;
                iteratorCount = count;
            }
        }
        count++;
        return iteratorCount;
    }
}
