using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(CharacterStats))]
public class Enemy : MonoBehaviour
{
    private FSMController fsm;

    protected CharacterStats characterStats;

    protected GameObject attackTarget;

    protected CharacterStats targetStats;

    public void Awake()
    {
        characterStats = GetComponent<CharacterStats>();
    }

    private void Start()
    {
        attackTarget = GameObject.FindGameObjectWithTag("Player");//�ҵ����
        targetStats = attackTarget.GetComponent<CharacterStats>();//��ȡ��ҵĻ�����ֵ
        InitFSM();
    }

    /// <summary>
    /// ��ʼ��״̬����ÿһ��״̬����������״̬���ת������
    /// </summary>
    void InitFSM()
    {
        fsm = new FSMController();

        //Ѳ��״̬
        FSMState patrolState = new PatrolState(fsm, gameObject, characterStats);
        patrolState.AddTransition(Transition.SeePlayer, StateID.Chase);
        patrolState.AddTransition(Transition.PlayerDead, StateID.Vitory);
        patrolState.AddTransition(Transition.Death, StateID.Dead);

        //����״̬
        FSMState guardState = new GuardState(fsm, gameObject, characterStats);
        guardState.AddTransition(Transition.SeePlayer, StateID.Chase);
        guardState.AddTransition(Transition.PlayerDead, StateID.Vitory);
        guardState.AddTransition(Transition.Death, StateID.Dead);


        //׷��״̬
        FSMState chaseState = new ChaseState(fsm,gameObject,characterStats);
        chaseState.AddTransition(Transition.LostPlayer, StateID.Patrol);
        chaseState.AddTransition(Transition.NearPlayer, StateID.Attack);
        chaseState.AddTransition(Transition.Guard, StateID.Guard);
        chaseState.AddTransition(Transition.PlayerDead, StateID.Vitory);
        chaseState.AddTransition(Transition.Death, StateID.Dead);

        //����״̬
        FSMState attackState = new AttackState(fsm, gameObject,characterStats);
        attackState.AddTransition(Transition.SeePlayer, StateID.Chase);
        attackState.AddTransition(Transition.Death, StateID.Dead);
        attackState.AddTransition(Transition.PlayerDead, StateID.Vitory);

        //����״̬
        FSMState deadState = new DeadState(fsm, gameObject);

        //ʤ��״̬
        FSMState victoryState = new VictoryState(fsm, gameObject);

        //��ӵ��˵ĳ�ʼ״̬����������Ѳ��
        if (characterStats.isGuard)
        {
            fsm.AddState(guardState);
        }
        else
        {
            fsm.AddState(patrolState);
        }
        fsm.AddState(chaseState);
        fsm.AddState(attackState);
        fsm.AddState(deadState);
        fsm.AddState(victoryState);
    }
    private void Update()
    {
        //FSMÿ֡��Ҫ������
        fsm.Update();
    }

    /// <summary>
    /// �ڱ༭״̬�»������ӷ�Χ��Բ
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position,5);
    }

    /// <summary>
    /// ���������ص��¼������������˺�
    /// </summary>
    void Hit()
    {
        //�ж�����Ƿ���ǰ��һ���Ƕȷ�Χ��
        if (Vector3.Distance(transform.position, attackTarget.transform.position) < characterStats.AttackRange && 
            transform.IsFacingTarget(attackTarget.transform))
        {
            targetStats.TakeDamage(characterStats, targetStats);//���������˺�
        }
    }

    /// <summary>
    /// ���������ص��¼�
    /// </summary>
    void CriticalHit()
    {
        //�ж�����Ƿ���ǰ��һ���Ƕȷ�Χ��
        if (Vector3.Distance(transform.position, attackTarget.transform.position) < characterStats.AttackRange &&
            transform.IsFacingTarget(attackTarget.transform))
        {
            targetStats.TakeDamage(characterStats, targetStats);//���������˺�
            EventHandler.CallEnemyCriticalEvent();
        }
        characterStats.isCritical = false;//���Լ��ı�����ΪFalse��
    }
}
