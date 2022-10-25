using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class GAction : MonoBehaviour
{
    public string actionName = "Action";
    public float cost = 1f;
    public GameObject target;
    public string targetTag;
    public float duration = 0f;
    public WorldState[] preConditions;
    public WorldState[] afterEffects;
    public NavMeshAgent agent;
    public Dictionary<string, int> preconditions;
    public Dictionary<string, int> effects;

    public WorldStates agentBeliefs;

    public GInventory inventory;
    public bool running = false;

    public WorldStates beliefs;

    public GAction()
    {
        preconditions = new Dictionary<string, int>();
        effects = new Dictionary<string, int>();  
    }

    public void Awake()
    {
        agent = this.gameObject.GetComponent<NavMeshAgent>();

        if(preConditions != null)
        {
            foreach(WorldState preCondition in preConditions)
            {
                preconditions.Add(preCondition.key, preCondition.value);
            }
        }

        if (afterEffects != null)
        {
            foreach (WorldState effect in afterEffects)
            {
                effects.Add(effect.key, effect.value);
            }
        }

        inventory = this.GetComponent<GAgent>().inventory;
        beliefs = this.GetComponent<GAgent>().beliefs;
    }

    public bool IsAchievable()
    {
        return true;
    }

    public bool IsAchievableGiven(Dictionary<string, int> conditions)
    {
        foreach(KeyValuePair<string, int> p in preconditions)
        {
            if (!conditions.ContainsKey(p.Key))
            {
                return false;
            }
        }
        return true;
    }

    public abstract bool PrePerform();
    public abstract bool PostPerform();
}
