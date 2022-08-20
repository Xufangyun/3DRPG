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
    /// 第一次实例化要拿到需要的组件
    /// </summary>
    /// <param name="fSMController">状态机</param>
    /// <param name="npc">操作对象</param>
    /// <param name="characterStats">操作对象属性</param>
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
        //检测可视范围内是否有玩家，是就转换追击状态
        var colliders = Physics.OverlapSphere(npc.transform.position, sight);
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

    //获取随机巡逻点
    void NewRandomPoint()
    {
        remainLookAtTime = lookAtTime;
        var randomPoint = startPoint + UnityEngine.Random.insideUnitSphere * patrolRange;//获取半径patrolRange圆范围内的随即点
        NavMeshHit hit;
        wayPoint = NavMesh.SamplePosition(randomPoint, out hit, patrolRange, 1) ? hit.position :npc.transform.position;//检测目标点距离最近的的导航网格点，检测范围尽量合适（2倍）
    }

    
}
