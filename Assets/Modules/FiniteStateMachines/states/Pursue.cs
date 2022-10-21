using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pursue : State
{
    public Pursue(GameObject npc, NavMeshAgent agent, Animator anim, Transform player) : base(npc, agent, anim, player)
    {
        name = STATE.PURSUE;
        agent.speed = 4;
        agent.isStopped = false;
    }

    public override void Enter()
    {
        anim.SetTrigger("isRunning");
        base.Enter();
    }

    public override void Update()
    {
        agent.SetDestination(player.transform.position);
        if (agent.hasPath)
        {
            if (CanAttackPlayer())
            {
                nextstate = new Attack(npc, agent, anim, player);
                stage = EVENT.EXIT;
            }else if (!CanSeePlayer())
            {
                nextstate = new Patrol(npc, agent, anim, player);
                stage = EVENT.EXIT;
            }
        }

    }

    public override void Exit()
    {
        anim.ResetTrigger("isRunning");
        base.Exit();
    }
}

