using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolState : FSMState
{
    private Animator anim;
    private NavMeshAgent agent;
    private Vector3 wayPoint;
    private Vector3 startPoint;
    public float patrolRange;
    public float lookAtTime;
    private float remainLookAtTime;
    private float speed;
    private float sight;
    private GameObject npc;
    private bool isEndGame;
    private CharacterStats characterStats;

    /// <summary>
    /// ��һ��ʵ����Ҫ�õ���Ҫ�����
    /// </summary>
    /// <param name="fSMController">״̬��</param>
    /// <param name="npc">��������</param>
    /// <param name="characterStats">������������</param>
    public PatrolState(FSMController fSMController,GameObject npc,CharacterStats characterStats):base(fSMController)
    {
        this.npc = npc;
        stateID = StateID.Patrol;
        this.characterStats = characterStats;
        anim = npc.GetComponent<Animator>();
        agent = npc.GetComponent<NavMeshAgent>();
        startPoint = npc.transform.position;
        lookAtTime = characterStats.LookAtTime;
        patrolRange = characterStats.PatrolRange;
        speed = agent.speed;
        NewRandomPoint();
        remainLookAtTime = 2;
        sight = characterStats.SightRadius;
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
        agent.speed = speed * 0.5f;
        if (Vector3.Distance(npc.transform.position, wayPoint) < 1)
        {
            anim.SetBool("Walk", false);//isWalk = false;
            remainLookAtTime -= Time.deltaTime;
            if (remainLookAtTime < 0)
            {
                NewRandomPoint();
            }
        }
        else
        {
            anim.SetBool("Walk", true);//isWalk = true;
            agent.destination = wayPoint;
        }
    }

    public override void DoAfterLeave()
    {
        anim.SetBool("Walk", false);
        agent.speed = speed;
    }

    public override void Reason()
    {
        //�����ӷ�Χ���Ƿ�����ң��Ǿ�ת��׷��״̬
        var colliders = Physics.OverlapSphere(npc.transform.position, sight);
        foreach (var target in colliders)
        {
            if (target.CompareTag("Player"))
            {
                fSMController.PreformTransition(Transition.SeePlayer);
                return;
            }
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

    //��ȡ���Ѳ�ߵ�
    void NewRandomPoint()
    {
        remainLookAtTime = lookAtTime;
        var randomPoint = startPoint + UnityEngine.Random.insideUnitSphere * patrolRange;//��ȡ�뾶patrolRangeԲ��Χ�ڵ��漴��
        NavMeshHit hit;
        wayPoint = NavMesh.SamplePosition(randomPoint, out hit, patrolRange, 1) ? hit.position :npc.transform.position;//���Ŀ����������ĵĵ�������㣬��ⷶΧ�������ʣ�2����
    }

    
}
