using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackState : FSMState
{
    private NavMeshAgent agent;
    private Animator anim;
    private GameObject npc;
    public float attack_CD;
    private float timer;
    private GameObject attackTarget;
    private float attack_Range;
    private CharacterStats characterStats;
    private bool isEndGame;

    /// <summary>
    /// ��һ��ʵ����Ҫ�õ���Ҫ�����
    /// </summary>
    /// <param name="fSMController">״̬��</param>
    /// <param name="npc">��������</param>
    /// <param name="characterStats">������������</param>
    public AttackState(FSMController fSMController,GameObject npc,CharacterStats characterStats) :base(fSMController)
    {
        stateID = StateID.Attack;
        this.npc = npc;
        this.characterStats = characterStats;
        anim = npc.GetComponent<Animator>();
        agent = npc.GetComponent<NavMeshAgent>();
        attack_CD = characterStats.CoolDown;
        timer = 0;
        attack_Range = characterStats.AttackRange;
        
    }

    public override void DoBeforeEnter()
    {
        EventHandler.EndGameEvent += OnEndGame;//������Ϸ�����¼�
        EventHandler.PlayerCriticalEvent += OnPlayerCriticalEvent;
        anim.SetBool("Battle", true);
        attackTarget = GameObject.FindGameObjectWithTag("Player");
    }

    public override void Act()
    {
        timer -= Time.deltaTime;//��ʱ��
        if (timer < 0)
        {
            npc.transform.LookAt(attackTarget.transform);

            //Golem:���еļ���
            if (Vector3.Distance(attackTarget.transform.position,npc.transform.position) <characterStats.SkillRange&&
                Vector3.Distance(attackTarget.transform.position, npc.transform.position) > characterStats.AttackRange)
            {
                anim.SetTrigger("Skill");
                timer = attack_CD;
                return;
            }

            //��ͨ�������߱���
            characterStats.isCritical = UnityEngine.Random.value < characterStats.CriticalChance;
            if (characterStats.isCritical)
            {
                anim.SetTrigger("Critical");
            }
            else anim.SetTrigger("Attack");
            timer = attack_CD;
        }
    }

    public override void DoAfterLeave()
    {
        EventHandler.PlayerCriticalEvent -= OnPlayerCriticalEvent;
        anim.SetBool("Battle", false);
        attackTarget = null;
    }

    private void OnPlayerCriticalEvent(GameObject attackTarget)
    {
        if(npc==attackTarget)
            anim.SetTrigger("Hit");
    }

    //������Ϸ�����¼�
    private void OnEndGame()
    {
        isEndGame = true;
    }

    public override void Reason()
    {
        //����ܿ��˹������뷶Χ��ת��׷��״̬
        if(Vector3.Distance(npc.transform.position, attackTarget.transform.position) > attack_Range&&!characterStats.isGolem)
        {
            fSMController.PreformTransition(Transition.SeePlayer);
            return;
        }

        //Golem�����˼��ܾ��룬ת��׷��״̬
        if (Vector3.Distance(npc.transform.position, attackTarget.transform.position) > characterStats.SkillRange&&characterStats.isGolem)
        {
            fSMController.PreformTransition(Transition.SeePlayer);
            return;
        }

        //ս��״̬�£�����һ��ܣ�ת������״̬
        if (characterStats.CurrentHealth == 0)
        {
            fSMController.PreformTransition(Transition.Death);
            return;
        }

        //������ң�ת��ʤ��״̬
        if (isEndGame)
        {
            fSMController.PreformTransition(Transition.PlayerDead);
            return;
        }
    }

}
