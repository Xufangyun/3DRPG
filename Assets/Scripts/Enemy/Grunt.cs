using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Grunt : Enemy
{
    [Header("Skill")]
    public float kicForce;

    /// <summary>
    /// ţͷ�˻�����ҵļ���
    /// </summary>
    public void KicForce()
    {
        //�ж�����Ƿ���ǰ��һ���Ƕȷ�Χ��
        if (Vector3.Distance(transform.position, attackTarget.transform.position) < characterStats.AttackRange &&
            transform.IsFacingTarget(attackTarget.transform))
        {
            //������ң�����������ѣ��,�۳����Ѫ��
            Vector3 direction = (attackTarget.transform.position - transform.position).normalized;
            EventHandler.CallPlayerDizzyEvent(direction, kicForce);
            targetStats.TakeDamage(characterStats, targetStats);//���������˺�
        }
    }
}
