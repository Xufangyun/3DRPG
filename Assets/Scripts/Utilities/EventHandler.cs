using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class EventHandler
{
    //��������¼�
    public static event Action<Vector3> GroundClickedEvent;
    public static void CallGroundClickedEvent(Vector3 target)
    {
        GroundClickedEvent?.Invoke(target);
    }

    //��������¼�
    public static event Action<GameObject> EnemyClickedEvent;
    public static void CallEnemyClickedEvent(GameObject attackTarget)
    {
        EnemyClickedEvent?.Invoke(attackTarget);
    }

    //��Ϸ�����¼�
    public static event Action EndGameEvent;
    public static void CallEndGame()
    {
        EndGameEvent?.Invoke();
    }

    //���˱����¼�
    public static event Action EnemyCriticalEvent;
    public static void CallEnemyCriticalEvent()
    {
        EnemyCriticalEvent?.Invoke();
    }

    //����ʹ�ü��ܣ�ѣ�����,����������¼�
    public static event Action<Vector3,float> PlayerDizzyEvent;
    public static void CallPlayerDizzyEvent(Vector3 direction,float kicForce)
    {
        PlayerDizzyEvent?.Invoke(direction,kicForce);
    }

    //��ұ����¼�
    public static event Action<GameObject> PlayerCriticalEvent;
    public static void CallPlayerCriticalEvent(GameObject attackTarget)
    {
        PlayerCriticalEvent?.Invoke(attackTarget);
    }

    //���µ��˻������Ѫ��
    public static event Action<CharacterStats> UpdateHealthUI;
    public static void CallUpdateHealthUI(CharacterStats targetStats)
    {
        UpdateHealthUI?.Invoke(targetStats);
    }


    //������Ҿ���ֵ
    public static Action<CharacterStats> UpdatePlayerExpUI;
    public static void CallUpdatePlayerExpUI(CharacterStats playerStats)
    {
        UpdatePlayerExpUI?.Invoke(playerStats);
    }

    //���ı�ѡ�еĵ��˵�Shder����
    public static Action<GameObject> SelectedEnemyShaderChangeEvent;
    public static void CallSelectedEnemyShaderChangeEvent(GameObject target)
    {
        SelectedEnemyShaderChangeEvent?.Invoke(target);
    }

    //ȡ��ѡ�񣬻ָ�Ĭ��Shder
    public static Action CancelSelectedChangeShaderEvent;
    public static void CallCancelSelectedChangeShaderEvent()
    {
        CancelSelectedChangeShaderEvent?.Invoke();
    }

    public static Action<string> ShowDialogue;

    public static void CallShowDialogue(string dialogue)
    {
        ShowDialogue?.Invoke(dialogue);
    }
}
