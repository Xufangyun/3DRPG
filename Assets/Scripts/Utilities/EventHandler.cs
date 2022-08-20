using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class EventHandler
{
    //点击地面事件
    public static event Action<Vector3> GroundClickedEvent;
    public static void CallGroundClickedEvent(Vector3 target)
    {
        GroundClickedEvent?.Invoke(target);
    }

    //点击敌人事件
    public static event Action<GameObject> EnemyClickedEvent;
    public static void CallEnemyClickedEvent(GameObject attackTarget)
    {
        EnemyClickedEvent?.Invoke(attackTarget);
    }

    //游戏结束事件
    public static event Action EndGameEvent;
    public static void CallEndGame()
    {
        EndGameEvent?.Invoke();
    }

    //敌人暴击事件
    public static event Action EnemyCriticalEvent;
    public static void CallEnemyCriticalEvent()
    {
        EnemyCriticalEvent?.Invoke();
    }

    //敌人使用技能，眩晕玩家,并击退玩家事件
    public static event Action<Vector3,float> PlayerDizzyEvent;
    public static void CallPlayerDizzyEvent(Vector3 direction,float kicForce)
    {
        PlayerDizzyEvent?.Invoke(direction,kicForce);
    }

    //玩家暴击事件
    public static event Action<GameObject> PlayerCriticalEvent;
    public static void CallPlayerCriticalEvent(GameObject attackTarget)
    {
        PlayerCriticalEvent?.Invoke(attackTarget);
    }

    //更新敌人或者玩家血量
    public static event Action<CharacterStats> UpdateHealthUI;
    public static void CallUpdateHealthUI(CharacterStats targetStats)
    {
        UpdateHealthUI?.Invoke(targetStats);
    }


    //更新玩家经验值
    public static Action<CharacterStats> UpdatePlayerExpUI;
    public static void CallUpdatePlayerExpUI(CharacterStats playerStats)
    {
        UpdatePlayerExpUI?.Invoke(playerStats);
    }

    //更改被选中的敌人的Shder参数
    public static Action<GameObject> SelectedEnemyShaderChangeEvent;
    public static void CallSelectedEnemyShaderChangeEvent(GameObject target)
    {
        SelectedEnemyShaderChangeEvent?.Invoke(target);
    }

    //取消选择，恢复默认Shder
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
