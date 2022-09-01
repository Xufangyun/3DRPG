using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    //public CharacterData_SO templateData;

    public CharacterData_SO characterData;

    public AttackData_SO attackData;

    [HideInInspector]
    public bool isCritical;

    [Tooltip("是否守卫状态")]
    public bool isGuard;

    [Header("是Golem才需要勾上")]
    public bool isGolem;

    [HideInInspector]
    public bool isImmune;

    private void Awake()
    {
        //if(templateData!=null)
        //    characterData = Instantiate(templateData);
    }

    #region Read from CharacterData_SO
    public int MaxHealth
    {
        get { if (characterData != null) return characterData.maxHealth; else return 0; }
        set { characterData.maxHealth = value; }
    }
    public int CurrentHealth
    {
        get { if (characterData != null) return characterData.currentHealth; else return 0; }
        set { characterData.currentHealth = value; }
    }
    public int BaseDefence
    {
        get { if (characterData != null) return characterData.baseDefence; else return 0; }
        set { characterData.baseDefence = value; }
    }
    public int CurrentDefence
    {
        get { if (characterData != null) return characterData.currentDefence; else return 0; }
        set { characterData.currentDefence = value; }
    }
    public float SightRadius
    {
        get { if (characterData != null) return characterData.sightRadius; else return 0; }
        set { characterData.sightRadius = value; }
    }
    public float PatrolRange
    {
        get { if (characterData != null) return characterData.patrolRange; else return 0; }
        set { characterData.patrolRange = value; }
    }
    public float LookAtTime
    {
        get { if (characterData != null) return characterData.lookAtTime; else return 0; }
        set { characterData.lookAtTime = value; }
    }

    #endregion

    #region Read from AttackData_SO
    public float AttackRange
    {
        get { if (attackData != null) return attackData.attackRange; else return 0; }
        set { attackData.attackRange = value; }
    }
    public float SkillRange
    {
        get { if (attackData != null) return attackData.skillRange; else return 0; }
        set { attackData.skillRange = value; }
    }
    public float CoolDown
    {
        get { if (attackData != null) return attackData.coolDown; else return 0; }
        set { attackData.coolDown = value; }
    }
    public int MinDamge
    {
        get { if (attackData != null) return attackData.minDamge; else return 0; }
        set { attackData.minDamge = value; }
    }
    public int MaxDamge
    {
        get { if (attackData != null) return attackData.maxDamge; else return 0; }
        set { attackData.maxDamge = value; }
    }
    public float CriticalMultiply
    {
        get { if (attackData != null) return attackData.criticalMultiply; else return 0; }
        set { attackData.criticalMultiply = value; }
    }
    public float CriticalChance
    {
        get { if (attackData != null) return attackData.criticalChance; else return 0; }
        set { attackData.criticalChance = value; }
    }
    #endregion

    /// <summary>
    /// 敌人进行攻击时触发，自己执行此函数，扣除自己的血量
    /// </summary>
    /// <param name="attacker">敌人攻击数值</param>
    /// <param name="defender">自身防御力</param>
    public void TakeDamage(CharacterStats attacker,CharacterStats defender)
    {
        if (isImmune) return;//如果玩家开启保护罩，进入免疫状态不造成伤害
        int damage = Mathf.Max(attacker.CurrentDamage()-defender.CurrentDefence, 0);
        CurrentHealth = Mathf.Max(CurrentHealth - damage,0);
        
        if (CurrentHealth <= 0)
        {
            attacker.characterData.UpdateExp(characterData.killExp);
            EventHandler.CallUpdatePlayerExpUI(attacker);
            EventHandler.CallUpdateHealthUI(attacker);
        }
        EventHandler.CallUpdateHealthUI(defender);
    }

    /// <summary>
    /// 石头攻击
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="defener"></param>
    public void TakeDamage(int damage,CharacterStats defener)
    {
        if (isImmune) return;
        int currentDamage = Mathf.Max(damage - defener.CurrentDefence, 0);
        CurrentHealth = Mathf.Max(CurrentHealth - currentDamage, 0);
        EventHandler.CallUpdateHealthUI(defener);
    }

    /// <summary>
    /// 自身攻击数值计算，暴击或没暴击
    /// </summary>
    /// <returns></returns>
    private int CurrentDamage()
    {
        float coreDamage =Random.Range(attackData.minDamge, attackData.maxDamge);
        if (isCritical)
        {
            coreDamage *= attackData.criticalMultiply;
        }
        return (int)coreDamage;
    }
}
