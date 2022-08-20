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
    /// ʯͷ�˻�����Ҽ���
    /// </summary>
    public void KicForce()
    {
        //�ж�����Ƿ���ǰ��һ���Ƕȷ�Χ��
        if (Vector3.Distance(transform.position, attackTarget.transform.position) < characterStats.AttackRange &&
            transform.IsFacingTarget(attackTarget.transform)) 
        {
            //������ң�����������ѣ��,�۳����Ѫ��
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
