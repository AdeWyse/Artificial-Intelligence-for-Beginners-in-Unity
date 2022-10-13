using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WpFollow : MonoBehaviour
{
    Transform goal;
    float speed = 5f;
    float accuracy = 5f;
    float rotSpeed = 2f;

    public GameObject wpManager;
    GameObject[] waypoints;
    GameObject currentNode;
    int currentWp = 0;
    Graph g;

    bool move = false;

    // Start is called before the first frame update
    void Start()
    {
        waypoints = wpManager.GetComponent<WpManager>().waypoints;
        g = wpManager.GetComponent<WpManager>().graph;
        
        currentNode = waypoints[16];
    }

    public void GoToHeli()
    {
        g.AStar(currentNode, waypoints[9]);
        currentWp = 0;
        move = true;
    }

    public void GoToBuilding()
    {
        g.AStar(currentNode, waypoints[13]);
        currentWp = 0;
        move = true;
    }

    public void GoToCity()
    {
        g.AStar(currentNode, waypoints[5]);
        currentWp = 0;

        move = true;
    }

    public void GoToRuin()
    {
        g.AStar(currentNode, waypoints[3]);
        currentWp = 0;
        move = true;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (move){
            if (g.pathList.Count == 0 || currentWp == g.pathList.Count)
            {
                move = false;
                return;
            }

            if (Vector3.Distance(g.pathList[currentWp].getid().transform.position, this.transform.position) < accuracy)
            {
                currentNode = g.pathList[currentWp].getid();
                currentWp++;
            }

            if (currentWp < g.pathList.Count)
            {
                goal = g.pathList[currentWp].getid().transform;
                Vector3 lookAtGoal = new Vector3(goal.position.x, this.transform.position.y, goal.position.z);
                Vector3 direction = lookAtGoal - this.transform.position;
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), rotSpeed * Time.deltaTime);
                this.transform.Translate(0, 0, speed * Time.deltaTime);
            }
        }
        
       
    }
}
