using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node 
{
   public List<Edge> edgeList =  new List<Edge>();
    public Node path = null;
    GameObject id;
    /*public float xPos;
    public float yPos;
    public float zPos;*/

    public float f, g, h;
    public Node cameFrom;

    public Node (GameObject i)
    {
        id = i;
       /* xPos = id.transform.position.x;
        yPos = id.transform.position.y;
        zPos = id.transform.position.z;*/
        path = null;
    }

    public GameObject getid()
    {
        return id;
    }


    
}
