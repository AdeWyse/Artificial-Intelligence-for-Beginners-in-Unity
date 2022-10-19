using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RunAway : State
{
    Transform safeArea;


    public RunAway(GameObject npc, NavMeshAgent agent, Animator anim, Transform player) : base(npc, agent, anim, player)
    {
        name = STATE.RUNAWAY;
        agent.speed = 4;
        agent.isStopped = false;
        safeArea = GameObject.FindGameObjectWithTag("Safe").GetComponent<Transform>();
    }

    public override void Enter()
    {
        anim.SetTrigger("isRunning");
        agent.isStopped = false;
        agent.SetDestination(safeArea.position);

        base.Enter();
    }

    public override void Update()
    {
        float distance = Vector3.Distance(safeArea.position, npc.transform.position);


        if (distance < 1)
        {
            nextstate = new Idle(npc, agent, anim, player);
            stage = EVENT.EXIT;
        }

    }

    public override void Exit()
    {
        anim.ResetTrigger("isRunning");
        base.Exit();
    }
}

