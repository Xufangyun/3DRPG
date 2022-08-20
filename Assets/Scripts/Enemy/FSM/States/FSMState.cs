using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 转换条件
/// </summary>
public enum Transition
{
    NullTransition,//空的转条件
    SeePlayer,//看到玩家
    LostPlayer,//追赶过程中丢失玩家目标
    NearPlayer,//靠近玩家执行攻击
    Guard,//守卫状态的敌人追击玩家丢失玩家目标时
    Death,//敌人死亡了
    PlayerDead,//玩家死亡了，敌人胜利
}

/// <summary>
/// 当前状态
/// </summary>
public enum StateID
{
    NullState,//空状态
    Patrol,//巡逻状态
    Chase,//追赶状态
    Attack,//攻击状态
    Guard,//守卫状态
    Dead,//死亡状态
    Vitory,//胜利状态
}
public abstract class FSMState
{
    protected StateID stateID;
    public StateID StateID { get { return stateID; } }
    protected Dictionary<Transition, StateID> transitionStateDic = new Dictionary<Transition, StateID>();
    protected FSMController fSMController;

    //实例化时拿到FSMController
    public FSMState(FSMController fSMController)
    {
        this.fSMController = fSMController;
    }

    /// <summary>
    /// 添加转换条件
    /// </summary>
    /// <param name="transition">转换条件</param>
    /// <param name="id">满足条件时需要转换的状态</param>
    public void AddTransition(Transition transition,StateID id)
    {
        if (transition == Transition.NullTransition)
        {
            Debug.LogError("不允许NullTransition");
            return;
        }
        if (id == StateID.NullState)
        {
            Debug.LogError("不允许NullStateID");
            return;
        }
        if (transitionStateDic.ContainsKey(transition))
        {
            Debug.LogError("添加转换条件的时候" + transition + "已经存在于transitionStateDic中");
            return;
        }
        transitionStateDic.Add(transition, id);
    }

    /// <summary>
    /// 删除转换条件
    /// </summary>
    /// <param name="transition">需要删除的条件</param>
    public void DeleteTransition(Transition transition)
    {
        if (transition == Transition.NullTransition)
        {
            Debug.LogError("不允许删除NullTransition");
            return;
        }
        if (!transitionStateDic.ContainsKey(transition))
        {
            Debug.LogError(transition + "不存在");
            return;
        }
        transitionStateDic.Remove(transition);
    }

    /// <summary>
    /// 获取当前条件下的状态
    /// </summary>
    /// <param name="transition"></param>
    /// <returns></returns>
    public StateID GetStateID(Transition transition)
    {
        if (transitionStateDic.ContainsKey(transition))
        {
            return transitionStateDic[transition];
        }
        return StateID.NullState;
    }

    //进入新状态之前做的事
    public virtual void DoBeforeEnter() { }

    //离开状态时做的事
    public virtual void DoAfterLeave() { }

    //当前状态所做的事
    public virtual void Act() { }

    //在某一状态执行中，新的转换条件满足时做的事
    public virtual void Reason() { }
}
