using UnityEngine;
using UnityEngine.AI;

public class Bot : MonoBehaviour
{
    NavMeshAgent agent;
    public GameObject target;
    DriveSteering drive;

    public float wanderRadius = 10f;
    public float wanderDistance = 20f;
    public float wanderJitter = 1f;

    Vector3 wanderTarget = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        drive = target.GetComponent<DriveSteering>();
    }

    void Seek(Vector3 location)
    {
        agent.SetDestination(location);
    }

    void Flee(Vector3 location)
    {
        Vector3 fleeVector = location - this.transform.position;
        agent.SetDestination(this.transform.position - fleeVector);
    }

    void Pursue()
    {
        Vector3 targetDirection = target.transform.position - this.transform.position;

        float relativeHeading = Vector3.Angle(this.transform.forward, this.transform.TransformVector(target.transform.forward));
        float toTarget = Vector3.Angle(this.transform.forward, this.transform.TransformVector(targetDirection));

        if ((toTarget > 90 && relativeHeading < 20) || drive.currentSpeed < 0.01f)
        {
            Seek(target.transform.position);
            return;
        }

        float lookAhead = targetDirection.magnitude / (agent.speed - drive.currentSpeed);

        Seek(target.transform.position + target.transform.forward * lookAhead);
    }

    void Evade()
    {
        Vector3 targetDirection = target.transform.position - this.transform.position;

        float lookAhead = targetDirection.magnitude / (agent.speed - drive.currentSpeed);

        Flee(target.transform.position + target.transform.forward * lookAhead);
    }


    void Wander()
    {
        wanderTarget = new Vector3(Random.Range(-1f, 1f) * wanderJitter, 0, Random.Range(-1f, 1f) * wanderJitter);

        wanderTarget.Normalize();
        wanderTarget *= wanderRadius;

        Vector3 targetLocal = wanderTarget + new Vector3(0, 0, wanderDistance);
        Vector3 targetWorld = this.gameObject.transform.InverseTransformVector(targetLocal);

        Seek(targetWorld);
    }

    void Hide()
    {
        float distance = Mathf.Infinity;

        Vector3 chosenSpot = Vector3.zero;

        for (int i = 0; i < World.Instance.GetHidingSpots().Length; i++)
        {
            Vector3 hideDirection = World.Instance.GetHidingSpots()[i].transform.position - target.transform.position;
            Vector3 hidePosition = World.Instance.GetHidingSpots()[i].transform.position + hideDirection.normalized * 10f;

            if (Vector3.Distance(this.transform.position, hidePosition) < distance)
            {
                chosenSpot = hidePosition;
                distance = Vector3.Distance(this.transform.position, hidePosition);
                ;
            }

        }
        Seek(chosenSpot);
    }

    void CleverHide()
    {
        float distance = Mathf.Infinity;

        Vector3 chosenSpot = Vector3.zero;

        Vector3 chosenDirection = Vector3.zero;
        GameObject chosenGO = World.Instance.GetHidingSpots()[0];

        for (int i = 0; i < World.Instance.GetHidingSpots().Length; i++)
        {
            Vector3 hideDirection = World.Instance.GetHidingSpots()[i].transform.position - target.transform.position;
            Vector3 hidePosition = World.Instance.GetHidingSpots()[i].transform.position + hideDirection.normalized * 10f;

            if (Vector3.Distance(this.transform.position, hidePosition) < distance)
            {
                chosenSpot = hidePosition;
                chosenDirection = hideDirection;
                chosenGO = World.Instance.GetHidingSpots()[i];
                distance = Vector3.Distance(this.transform.position, hidePosition);
            }

        }

        Collider hideCollider = chosenGO.GetComponent<Collider>();
        Ray backRay = new Ray(chosenSpot, -chosenDirection.normalized);
        RaycastHit info;
        float dis = 100f;
        hideCollider.Raycast(backRay, out info, dis);

        Seek(info.point + chosenDirection.normalized * 5);
    }

    bool CanSeeTarget()
    {
        RaycastHit info;
        Vector3 rayToTarget = target.transform.position - this.transform.position;
        if (Physics.Raycast(this.transform.position, rayToTarget, out info))
        {
            if (info.transform.gameObject.tag == "cop")
            {
                float angle = Vector3.Angle(rayToTarget, this.transform.forward);

                if (angle < 90)
                {
                    return true;
                }
                return false;
            }
        }
        return false;
    }

    bool CanSeeMe()
    {
        Vector3 rayToTarget = this.transform.position - target.transform.position;

        float angle = Vector3.Angle(target.transform.forward, rayToTarget);

        if (angle < 60)
        {
            return true;
        }
        return false;
    }

    bool coolDown = false;

    void BehaviourCooldown()
    {
        coolDown = false;
    }

    bool CheckDistance()
    {
        float distance = Vector3.Distance(this.transform.position, target.transform.position);

        if(distance > 10f)
        {
            return true;
        }
        return false;
    }
    // Update is called once per frame
    void Update()
    {
        if (CheckDistance())
        {
            Wander();
        }
        else
        {
            if (!coolDown)
            {
                if (CanSeeTarget() && CanSeeMe())
                {
                    CleverHide();
                    coolDown = true;
                    Invoke("BehaviourCooldown", 5);
                }
                else
                {
                    Pursue();
                }
            }
        }
    }
}
