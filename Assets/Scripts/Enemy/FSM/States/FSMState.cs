using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ת������
/// </summary>
public enum Transition
{
    NullTransition,//�յ�ת����
    SeePlayer,//�������
    LostPlayer,//׷�Ϲ����ж�ʧ���Ŀ��
    NearPlayer,//�������ִ�й���
    Guard,//����״̬�ĵ���׷����Ҷ�ʧ���Ŀ��ʱ
    Death,//����������
    PlayerDead,//��������ˣ�����ʤ��
}

/// <summary>
/// ��ǰ״̬
/// </summary>
public enum StateID
{
    NullState,//��״̬
    Patrol,//Ѳ��״̬
    Chase,//׷��״̬
    Attack,//����״̬
    Guard,//����״̬
    Dead,//����״̬
    Vitory,//ʤ��״̬
}
public abstract class FSMState
{
    protected StateID stateID;
    public StateID StateID { get { return stateID; } }
    protected Dictionary<Transition, StateID> transitionStateDic = new Dictionary<Transition, StateID>();
    protected FSMController fSMController;

    //ʵ����ʱ�õ�FSMController
    public FSMState(FSMController fSMController)
    {
        this.fSMController = fSMController;
    }

    /// <summary>
    /// ���ת������
    /// </summary>
    /// <param name="transition">ת������</param>
    /// <param name="id">��������ʱ��Ҫת����״̬</param>
    public void AddTransition(Transition transition,StateID id)
    {
        if (transition == Transition.NullTransition)
        {
            Debug.LogError("������NullTransition");
            return;
        }
        if (id == StateID.NullState)
        {
            Debug.LogError("������NullStateID");
            return;
        }
        if (transitionStateDic.ContainsKey(transition))
        {
            Debug.LogError("���ת��������ʱ��" + transition + "�Ѿ�������transitionStateDic��");
            return;
        }
        transitionStateDic.Add(transition, id);
    }

    /// <summary>
    /// ɾ��ת������
    /// </summary>
    /// <param name="transition">��Ҫɾ��������</param>
    public void DeleteTransition(Transition transition)
    {
        if (transition == Transition.NullTransition)
        {
            Debug.LogError("������ɾ��NullTransition");
            return;
        }
        if (!transitionStateDic.ContainsKey(transition))
        {
            Debug.LogError(transition + "������");
            return;
        }
        transitionStateDic.Remove(transition);
    }

    /// <summary>
    /// ��ȡ��ǰ�����µ�״̬
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

    //������״̬֮ǰ������
    public virtual void DoBeforeEnter() { }

    //�뿪״̬ʱ������
    public virtual void DoAfterLeave() { }

    //��ǰ״̬��������
    public virtual void Act() { }

    //��ĳһ״ִ̬���У��µ�ת����������ʱ������
    public virtual void Reason() { }
}
