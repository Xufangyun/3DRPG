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
    /// 第一次实例化要拿到需要的组件
    /// </summary>
    /// <param name="fSMController">状态机</param>
    /// <param name="npc">操作对象</param>
    /// <param name="characterStats">操作对象属性</param>
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
        //攻击状态
        if (Vector3.Distance(npc.transform.position, attackTarget.transform.position) < attack_Range&&!characterStats.isGolem)
        {
            fSMController.PreformTransition(Transition.NearPlayer);
            return;
        }

        //Golem攻击状态
        if (Vector3.Distance(npc.transform.position, attackTarget.transform.position) < characterStats.SkillRange && characterStats.isGolem)
        {
            fSMController.PreformTransition(Transition.NearPlayer);
            return;
        }

        //守卫状态
        if (Vector3.Distance(npc.transform.position, attackTarget.transform.position) > characterStats.SightRadius&&isGuard)
        {
            fSMController.PreformTransition(Transition.Guard);
            return;
        }
        //巡逻状态
        if (Vector3.Distance(npc.transform.position, attackTarget.transform.position) > characterStats.SightRadius && !isGuard)
        {
            fSMController.PreformTransition(Transition.LostPlayer);
            return;
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
