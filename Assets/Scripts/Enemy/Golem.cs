using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Golem : Enemy
{
    [Header("Skill")]
    public float force;

    public GameObject rockPrefab;

    public Transform handPos;

    /// <summary>
    /// 石头人击退玩家技能
    /// </summary>
    public void KicForce()
    {
        //判断玩家是否在前方一定角度范围内
        if (Vector3.Distance(transform.position, attackTarget.transform.position) < characterStats.AttackRange &&
            transform.IsFacingTarget(attackTarget.transform)) 
        {
            //击退玩家，并对玩家造成眩晕,扣除玩家血量
            Vector3 direction = (attackTarget.transform.position - transform.position).normalized;
            EventHandler.CallPlayerDizzyEvent(direction, force);
            targetStats.TakeDamage(characterStats, targetStats);
        }
    }

    public void ThrowRock()
    {
        if (attackTarget != null)
        {
            var rock = Instantiate(rockPrefab, handPos.position, Quaternion.identity);
            rock.GetComponent<Rock>().target = attackTarget;
        }
    }
}
