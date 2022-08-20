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

    //�����״̬
    public void AddState(FSMState state)
    {
        if (state == null)
        {
            Debug.LogError("FSMState����Ϊ��");
            return;
        }
        if (currentState == null)
        {
            currentState = state;
            currentStateID = state.StateID;
        }
        if (stateDic.ContainsKey(state.StateID))
        {
            Debug.LogError(state.StateID + "�Ѿ�������");
            return;
        }
        stateDic.Add(state.StateID,state);
    }

    //ɾ��״̬
    public void DeleteState(StateID stateID)
    {
        if (stateID == StateID.NullState)
        {
            Debug.LogError("�޷�ɾ����״̬");
            return;
        }
        if (!stateDic.ContainsKey(stateID))
        {
            Debug.LogError("�޷�ɾ�������ڵ�״̬");
            return;
        }
        stateDic.Remove(stateID);
    }

    //ִ�й�����������ʱ����Ӧ״̬��������
    public void PreformTransition(Transition transition)
    {
        if (transition == Transition.NullTransition)
        {
            Debug.LogError("�޷�ִ�п�ת������");
            return;
        }
        StateID id = currentState.GetStateID(transition);
        if (!stateDic.ContainsKey(id))
        {
            Debug.LogError("״̬���ﲻ����" + id+"�޷�ִ��״̬ת��");
            return;
        }
        if (transition == currentTrans)//����ͬһ��״̬�����ټ���ִ��
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
