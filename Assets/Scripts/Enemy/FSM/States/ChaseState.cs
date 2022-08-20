using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseState : FSMState
{
    private Animator anim;
    private NavMeshAgent agent;
    private GameObject npc;
    private GameObject attackTarget;
    private float attack_Range;
    private bool isGuard;
    private bool isEndGame;
    private CharacterStats characterStats;

    /// <summary>
    /// ��һ��ʵ����Ҫ�õ���Ҫ�����
    /// </summary>
    /// <param name="fSMController">״̬��</param>
    /// <param name="npc">��������</param>
    /// <param name="characterStats">������������</param>
    public ChaseState(FSMController fSMController,GameObject npc,CharacterStats characterStats) : base(fSMController)
    {
        stateID = StateID.Chase;
        this.npc = npc;
        this.characterStats = characterStats;
        anim = npc.GetComponent<Animator>();
        agent = npc.GetComponent<NavMeshAgent>();
        attackTarget = GameObject.FindGameObjectWithTag("Player");
        attack_Range = characterStats.AttackRange;
        isGuard = characterStats.isGuard;
        EventHandler.EndGameEvent += OnEndGame;//������Ϸ�����¼�
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
        anim.SetBool("Chase", true);//isChase = true;
        agent.destination = attackTarget.transform.position;
    }

    public override void DoAfterLeave()
    {
        anim.SetBool("Chase",false);
        agent.destination = npc.transform.position;
    }
    
    public override void Reason()
    {
        //����״̬
        if (Vector3.Distance(npc.transform.position, attackTarget.transform.position) < attack_Range&&!characterStats.isGolem)
        {
            fSMController.PreformTransition(Transition.NearPlayer);
            return;
        }

        //Golem����״̬
        if (Vector3.Distance(npc.transform.position, attackTarget.transform.position) < characterStats.SkillRange && characterStats.isGolem)
        {
            fSMController.PreformTransition(Transition.NearPlayer);
            return;
        }

        //����״̬
        if (Vector3.Distance(npc.transform.position, attackTarget.transform.position) > characterStats.SightRadius&&isGuard)
        {
            fSMController.PreformTransition(Transition.Guard);
            return;
        }
        //Ѳ��״̬
        if (Vector3.Distance(npc.transform.position, attackTarget.transform.position) > characterStats.SightRadius && !isGuard)
        {
            fSMController.PreformTransition(Transition.LostPlayer);
            return;
        }
        //������ң�ת��ʤ��״̬
        if (isEndGame)
        {
            fSMController.PreformTransition(Transition.PlayerDead);
            return;
        }

        //Ѫ��Ϊ0���л�����״̬
        if (characterStats.CurrentHealth == 0)
        {
            fSMController.PreformTransition(Transition.Death);
            return;
        }
    }

}
