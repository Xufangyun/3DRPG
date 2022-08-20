using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Data",menuName ="Character Stats/Data")]
public class CharacterData_SO : ScriptableObject
{
    [Header("Stats Info")]
    public int maxHealth;

    public int currentHealth;

    public int baseDefence;

    public int currentDefence;

    public float sightRadius;

    public float patrolRange;

    public float lookAtTime;

    [Header("Kill")]
    public int killExp;

    [Header("Level")]
    public int currentLevel;

    public int  maxLevel;

    public int baseExp;

    public int currentExp;

    public float levelBuff;

    public void UpdateExp(int point)
    {
        currentExp += point;
        if (currentExp >= baseExp)
            LevelUp();
    }

    private void LevelUp()
    {
        currentLevel =Mathf.Clamp( currentLevel + 1,0,maxLevel);
        baseExp += (int)(baseExp * (1+levelBuff));

        maxHealth +=(int)(maxHealth*levelBuff);
        currentHealth = maxHealth;

        Debug.Log("LEVEL UP!" + currentLevel + "Max Health" + maxHealth);

    }
}
