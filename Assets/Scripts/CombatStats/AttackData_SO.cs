using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Data",menuName ="Attack Stats/Data")]
public class AttackData_SO : ScriptableObject
{
    public float attackRange;

    public float skillRange;

    public float coolDown;

    public int minDamge;

    public int maxDamge;

    public float criticalMultiply;

    public float criticalChance;
}
