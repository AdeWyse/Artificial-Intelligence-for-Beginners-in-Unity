using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DropCapsule : MonoBehaviour
{

    public GameObject obstacle;
    GameObject[] agents;
    // Start is called before the first frame update
    void Start()
    {
        agents = GameObject.FindGameObjectsWithTag("agent");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hitInfo;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray.origin, ray.direction, out hitInfo))
            {
                Instantiate(obstacle, hitInfo.point, obstacle.transform.rotation);
                foreach(GameObject agent in agents){
                    agent.GetComponent<AIController>().DetectNewObstacle(hitInfo.point);
                }
            }
        }
    }
}


/*Code to create object where clicked - Inside Update 
 * 
 * if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hitInfo;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray.origin, ray.direction, out hitInfo))
            {
                Instantiate(obstacle, hitInfo.point, obstacle.transform.rotation);
                
            }
        }
*
**
**/