using UnityEngine;
using UnityEngine.AI;

public class RobberBehaviour : MonoBehaviour
{

    BehaviourTree tree;
    public GameObject diamond;
    public GameObject van;
    public GameObject backDoor;
    public GameObject frontDoor;
    GameObject stolen;
    NavMeshAgent agent;

    public enum ActionState { IDLE, MOVING };
    ActionState state = ActionState.IDLE;

    [Range(0, 1000)] public int money = 800;

    public NodeBehaviour.Status treeStatus = NodeBehaviour.Status.RUNNING;

    // Start is called before the first frame update
    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();

        tree = new BehaviourTree();
        Sequence steal = new Sequence("Steal Something");
        Leaf hasMoney = new Leaf("Has Money", HasMoney);
        Leaf goToBackDoor = new Leaf("Go To Back Door", GoToBackDoor);
        Leaf goToDiamond = new Leaf("Go To Diamond", GoToDiamond);
        Leaf goToFrontDoor = new Leaf("Go To Front Door", GoToFrontDoor);
        Leaf goToVan = new Leaf("Go To Van", GoToVan);
        Selector openDoor = new Selector("Open Door");

        openDoor.AddChild(goToBackDoor);
        openDoor.AddChild(goToFrontDoor);

        steal.AddChild(hasMoney);
        steal.AddChild(openDoor);
        steal.AddChild(goToDiamond);
        steal.AddChild(openDoor);
        steal.AddChild(goToVan);
        tree.AddChild(steal);

        //tree.PrintTree();

        Time.timeScale = 5;


    }

    public NodeBehaviour.Status GoToBackDoor()
    {
        return GoToDoor(backDoor);
    }

    public NodeBehaviour.Status GoToFrontDoor()
    {
        return GoToDoor(frontDoor);
    }

    public NodeBehaviour.Status GoToDoor(GameObject door)
    {
        NodeBehaviour.Status s = GoToLocation(door.transform.position);
        if(s == NodeBehaviour.Status.SUCCESS)
        {
            if (!door.GetComponent<Lock>().isLocked)
            {
                door.SetActive(false);
                return NodeBehaviour.Status.SUCCESS;
            }
            return NodeBehaviour.Status.FAILURE;
        }
        else
        {
            return s;
        }
    }

    public NodeBehaviour.Status HasMoney()
    {
        if (money <= 500)
        {
            return NodeBehaviour.Status.SUCCESS;
        }
        return NodeBehaviour.Status.FAILURE;
    }
    public NodeBehaviour.Status GoToDiamond()
    {
        return StealDiamond(diamond);
    }

    public NodeBehaviour.Status StealDiamond(GameObject diamond)
    {
        NodeBehaviour.Status s = GoToLocation(diamond.transform.position);
        if (s == NodeBehaviour.Status.SUCCESS)
        {
            stolen = diamond;
            stolen.transform.parent = this.gameObject.transform;
                return NodeBehaviour.Status.SUCCESS;
            
        }
        else
        {
            return s;
        }
    }

    public NodeBehaviour.Status GoToVan()
    {

        return CheckSteal(van);
    }

    public NodeBehaviour.Status CheckSteal(GameObject van)
    {
        NodeBehaviour.Status s = GoToLocation(van.transform.position);
        if (s == NodeBehaviour.Status.SUCCESS)
        {
            money += 500;
            Destroy(stolen);
            return NodeBehaviour.Status.SUCCESS;

        }
        else
        {
            return s;
        }
        
    }

    public NodeBehaviour.Status GoToLocation(Vector3 destination)
    {
        float distanceToTarget = Vector3.Distance(destination, this.gameObject.transform.position);
        if (state == ActionState.IDLE)
        {
            agent.SetDestination(destination);
            state = ActionState.MOVING;
        }

        else if (distanceToTarget < 3)
        {
            state = ActionState.IDLE;
            return NodeBehaviour.Status.SUCCESS;
        }
        else if (Vector3.Distance(agent.pathEndPosition, destination) >= 6)
        {
            state = ActionState.IDLE;
            return NodeBehaviour.Status.FAILURE;
        }
        return NodeBehaviour.Status.RUNNING;

    }

    // Update is called once per frame
    void Update()
    {
        if (treeStatus != NodeBehaviour.Status.SUCCESS)
        {
            treeStatus = tree.Process();
        }
    }
}
