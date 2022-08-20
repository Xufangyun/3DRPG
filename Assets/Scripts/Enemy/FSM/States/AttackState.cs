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
    /// 第一次实例化要拿到需要的组件
    /// </summary>
    /// <param name="fSMController">状态机</param>
    /// <param name="npc">操作对象</param>
    /// <param name="characterStats">操作对象属性</param>
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
        EventHandler.EndGameEvent += OnEndGame;//监听游戏结束事件
        EventHandler.PlayerCriticalEvent += OnPlayerCriticalEvent;
        anim.SetBool("Battle", true);
        attackTarget = GameObject.FindGameObjectWithTag("Player");
    }

    public override void Act()
    {
        timer -= Time.deltaTime;//计时器
        if (timer < 0)
        {
            npc.transform.LookAt(attackTarget.transform);

            //Golem:特有的技能
            if (Vector3.Distance(attackTarget.transform.position,npc.transform.position) <characterStats.SkillRange&&
                Vector3.Distance(attackTarget.transform.position, npc.transform.position) > characterStats.AttackRange)
            {
                anim.SetTrigger("Skill");
                timer = attack_CD;
                return;
            }

            //普通攻击或者暴击
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

    //监听游戏结束事件
    private void OnEndGame()
    {
        isEndGame = true;
    }

    public override void Reason()
    {
        //玩家跑开了攻击距离范围，转换追击状态
        if(Vector3.Distance(npc.transform.position, attackTarget.transform.position) > attack_Range&&!characterStats.isGolem)
        {
            fSMController.PreformTransition(Transition.SeePlayer);
            return;
        }

        //Golem超过了技能距离，转换追击状态
        if (Vector3.Distance(npc.transform.position, attackTarget.transform.position) > characterStats.SkillRange&&characterStats.isGolem)
        {
            fSMController.PreformTransition(Transition.SeePlayer);
            return;
        }

        //战斗状态下，被玩家击败，转换死亡状态
        if (characterStats.CurrentHealth == 0)
        {
            fSMController.PreformTransition(Transition.Death);
            return;
        }

        //击败玩家，转换胜利状态
        if (isEndGame)
        {
            fSMController.PreformTransition(Transition.PlayerDead);
            return;
        }
    }

}
