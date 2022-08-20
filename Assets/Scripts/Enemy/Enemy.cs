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
        attackTarget = GameObject.FindGameObjectWithTag("Player");//找到玩家
        targetStats = attackTarget.GetComponent<CharacterStats>();//获取玩家的基础数值
        InitFSM();
    }

    /// <summary>
    /// 初始化状态机和每一个状态，并给各个状态添加转换条件
    /// </summary>
    void InitFSM()
    {
        fsm = new FSMController();

        //巡逻状态
        FSMState patrolState = new PatrolState(fsm, gameObject, characterStats);
        patrolState.AddTransition(Transition.SeePlayer, StateID.Chase);
        patrolState.AddTransition(Transition.PlayerDead, StateID.Vitory);
        patrolState.AddTransition(Transition.Death, StateID.Dead);

        //守卫状态
        FSMState guardState = new GuardState(fsm, gameObject, characterStats);
        guardState.AddTransition(Transition.SeePlayer, StateID.Chase);
        guardState.AddTransition(Transition.PlayerDead, StateID.Vitory);
        guardState.AddTransition(Transition.Death, StateID.Dead);


        //追击状态
        FSMState chaseState = new ChaseState(fsm,gameObject,characterStats);
        chaseState.AddTransition(Transition.LostPlayer, StateID.Patrol);
        chaseState.AddTransition(Transition.NearPlayer, StateID.Attack);
        chaseState.AddTransition(Transition.Guard, StateID.Guard);
        chaseState.AddTransition(Transition.PlayerDead, StateID.Vitory);
        chaseState.AddTransition(Transition.Death, StateID.Dead);

        //攻击状态
        FSMState attackState = new AttackState(fsm, gameObject,characterStats);
        attackState.AddTransition(Transition.SeePlayer, StateID.Chase);
        attackState.AddTransition(Transition.Death, StateID.Dead);
        attackState.AddTransition(Transition.PlayerDead, StateID.Vitory);

        //死亡状态
        FSMState deadState = new DeadState(fsm, gameObject);

        //胜利状态
        FSMState victoryState = new VictoryState(fsm, gameObject);

        //添加敌人的初始状态，守卫或者巡逻
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
        //FSM每帧需要做的事
        fsm.Update();
    }

    /// <summary>
    /// 在编辑状态下画出可视范围的圆
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position,5);
    }

    /// <summary>
    /// 攻击动画回调事件，对玩家造成伤害
    /// </summary>
    void Hit()
    {
        //判断玩家是否在前方一定角度范围内
        if (Vector3.Distance(transform.position, attackTarget.transform.position) < characterStats.AttackRange && 
            transform.IsFacingTarget(attackTarget.transform))
        {
            targetStats.TakeDamage(characterStats, targetStats);//给玩家造成伤害
        }
    }

    /// <summary>
    /// 暴击动画回调事件
    /// </summary>
    void CriticalHit()
    {
        //判断玩家是否在前方一定角度范围内
        if (Vector3.Distance(transform.position, attackTarget.transform.position) < characterStats.AttackRange &&
            transform.IsFacingTarget(attackTarget.transform))
        {
            targetStats.TakeDamage(characterStats, targetStats);//给玩家造成伤害
            EventHandler.CallEnemyCriticalEvent();
        }
        characterStats.isCritical = false;//将自己的暴击设为False；
    }
}
