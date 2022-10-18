using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Idle:State
{
    public Idle(GameObject npc, NavMeshAgent agent, Animator anim, Transform player) : base(npc, agent, anim, player)
    {
        name = STATE.IDLE;
    }

    public override void Enter()
    {
        anim.SetTrigger("isIdle");
         base.Enter();
    }

    public override void Update()
    {
        anim.SetTrigger("isIdle");
        if (CanSeePlayer())
        {
            nextstate = new Pursue(npc, agent, anim, player);
            stage = EVENT.EXIT;
        }
        if (Random.Range(0, 100) < 10)
        {
            nextstate = new Patrol(npc, agent, anim, player);
            
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        anim.ResetTrigger("isIdle");
        base.Exit();
    }

}
