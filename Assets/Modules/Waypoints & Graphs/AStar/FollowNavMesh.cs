using UnityEngine;
using UnityEngine.AI;

public class FollowNavMesh : MonoBehaviour
{
    public GameObject wpManager;
    GameObject[] waypoints;
    GameObject currentNode;

    NavMeshAgent agent;


    // Start is called before the first frame update
    void Start()
    {
        waypoints = wpManager.GetComponent<WpManager>().waypoints;

        currentNode = waypoints[16];

        agent = this.gameObject.gameObject.GetComponent<NavMeshAgent>();
    }

    public void GoToHeli()
    {
        agent.SetDestination(waypoints[9].transform.position);
    }

    public void GoToBuilding()
    {
        //g.AStar(currentNode, waypoints[13]);
        agent.SetDestination(waypoints[13].transform.position);
    }

    public void GoToCity()
    {
        //g.AStar(currentNode, waypoints[5]);
        agent.SetDestination(waypoints[5].transform.position);
    }

    public void GoToRuin()
    {
        // g.AStar(currentNode, waypoints[3]);
        agent.SetDestination(waypoints[2].transform.position);
    }

    // Update is called once per frame
    void LateUpdate()
    { 
        
    }
}
