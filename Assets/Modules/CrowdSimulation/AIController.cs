using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{

    GameObject[] goalLocations;
    NavMeshAgent agent;
    Animator animator;
    float speedMultiplier;

    float detectionRadius = 20f;
    float fleeRadius = 10f;
    // Start is called before the first frame update
    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        goalLocations = GameObject.FindGameObjectsWithTag("goal");
        int goalIndex = Random.Range(0, goalLocations.Length);
        agent.SetDestination(goalLocations[goalIndex].transform.position);
        animator = GetComponent<Animator>();
        animator.SetFloat("wOffset", Random.Range(0, 1f));
        ResetAgent();



    }

    void ResetAgent()
    {
        speedMultiplier = Random.Range(0.1f, 1.5f);
        animator.SetFloat("speedVult", speedMultiplier);
        agent.speed += speedMultiplier;
        agent.angularSpeed = 120f;
        animator.SetTrigger("isWalking");
        agent.ResetPath();
    }

    public void DetectNewObstacle(Vector3 position)
    {
        if(Vector3.Distance(position, this.transform.position) < detectionRadius)
        {
            Vector3 fleeDirection = (this.transform.position - position).normalized;
            Vector3 newGoal = this.transform.position + fleeDirection * fleeRadius;

            NavMeshPath path = new NavMeshPath();

            agent.CalculatePath(newGoal, path);

            if(path.status != NavMeshPathStatus.PathInvalid)
            {
                agent.SetDestination(path.corners[path.corners.Length - 1]);
                animator.SetTrigger("isRunning");
                agent.speed = 10f;
                agent.angularSpeed = 500f;
            }

            
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(agent.remainingDistance < 1)
        {
            ResetAgent();
            int goalIndex = Random.Range(0, goalLocations.Length);
            agent.SetDestination(goalLocations[goalIndex].transform.position);

        }
    }
}
