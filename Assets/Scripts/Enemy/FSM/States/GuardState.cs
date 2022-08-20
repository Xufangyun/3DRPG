using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardState : FSMState
{
    GameObject npc;
    CharacterStats characterStats;
    Animator anim;
    NavMeshAgent agent;
    Vector3 guardPos;
    Quaternion guardRotation;
    private bool isEndGame;

    public GuardState(FSMController fSMController,GameObject npc,CharacterStats characterStats):base(fSMController)
    {
        stateID = StateID.Guard;
        this.npc=npc;
        this.characterStats = characterStats;
        anim = npc.GetComponent<Animator>();
        agent = npc.GetComponent<NavMeshAgent>();
        guardPos = npc.transform.position;
        guardRotation = npc.transform.rotation;
        EventHandler.EndGameEvent += OnEndGame;//监听游戏结束事件
    }

    private void OnEndGame()
    {
        isEndGame = true;
    }

    public override void DoBeforeEnter()
    {
    }

    public override void Act()
    {
        if (npc.transform.position != guardPos)
        {
            anim.SetBool("Walk", true);
            agent.destination = guardPos;
            if (Vector3.SqrMagnitude(guardPos - npc.transform.position) <= agent.stoppingDistance)
            {
                anim.SetBool("Walk", false);
                npc.transform.rotation = Quaternion.Lerp(npc.transform.rotation, guardRotation, 0.01f);
            }
        }
    }

    public override void DoAfterLeave()
    {
    }

    public override void Reason()
    {
        //检测可视范围内是否有玩家，是就转换追击状态
        var colliders = Physics.OverlapSphere(npc.transform.position, characterStats.SightRadius);
        foreach (var target in colliders)
        {
            if (target.CompareTag("Player"))
            {
                fSMController.PreformTransition(Transition.SeePlayer);
                return;
            }
        }
        //击败玩家，转换胜利状态
        if (isEndGame)
        {
            fSMController.PreformTransition(Transition.PlayerDead);
            return;
        }
        //血量为0，切换死亡状态
        if (characterStats.CurrentHealth == 0)
        {
            fSMController.PreformTransition(Transition.Death);
            return;
        }
    }

}
