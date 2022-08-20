using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMController
{
    private Dictionary<StateID, FSMState> stateDic = new Dictionary<StateID, FSMState>();
    private StateID currentStateID;
    private FSMState currentState;
    private Transition currentTrans;

    public FSMController()
    {

    }
    public void Update()
    {
        currentState.Act();
        currentState.Reason();
    }

    //添加新状态
    public void AddState(FSMState state)
    {
        if (state == null)
        {
            Debug.LogError("FSMState不能为空");
            return;
        }
        if (currentState == null)
        {
            currentState = state;
            currentStateID = state.StateID;
        }
        if (stateDic.ContainsKey(state.StateID))
        {
            Debug.LogError(state.StateID + "已经存在了");
            return;
        }
        stateDic.Add(state.StateID,state);
    }

    //删除状态
    public void DeleteState(StateID stateID)
    {
        if (stateID == StateID.NullState)
        {
            Debug.LogError("无法删除空状态");
            return;
        }
        if (!stateDic.ContainsKey(stateID))
        {
            Debug.LogError("无法删除不存在的状态");
            return;
        }
        stateDic.Remove(stateID);
    }

    //执行过度条件满足时，对应状态该做的事
    public void PreformTransition(Transition transition)
    {
        if (transition == Transition.NullTransition)
        {
            Debug.LogError("无法执行空转换条件");
            return;
        }
        StateID id = currentState.GetStateID(transition);
        if (!stateDic.ContainsKey(id))
        {
            Debug.LogError("状态机里不存在" + id+"无法执行状态转换");
            return;
        }
        if (transition == currentTrans)//若是同一个状态，则不再继续执行
        {
            return;
        }
        if (currentTrans == Transition.NullTransition)
        {
            currentTrans = transition;
        }
        
        FSMState state = stateDic[id];
        currentState.DoAfterLeave();
        currentState = state;
        currentStateID = state.StateID;
        currentState.DoBeforeEnter();
        currentTrans = transition;
    }

}
