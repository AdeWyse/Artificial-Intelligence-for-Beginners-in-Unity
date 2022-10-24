using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Effects;

public abstract class GAction : MonoBehaviour
{
    public string actionName = "Action";
    public float cost = 1f;
    public GameObject target;
    public GameObject targetTag;
    public float duration = 0f;
    public WorldState[] preConditions;
    public WorldState[] afterEffects;
    public NavMeshAgent agent;
    public Dictionary<string, int> preconditions;
    public Dictionary<string, int> effects;

    public WorldStates agentBeliefs;
    public bool running = false;

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
