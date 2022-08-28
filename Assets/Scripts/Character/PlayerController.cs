using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    private CharacterStats characterStats;
    private NavMeshAgent agent;
    private Animator anim;
    //攻击目标
    private GameObject attackTarget;
    //攻击CD计时器
    private float lastAttackTime;

    private bool isDead;

    public GameObject energyMask;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        characterStats = GetComponent<CharacterStats>();
    }
    private void Start()
    {
        
        EventHandler.CallUpdateHealthUI(GameManager.Instance.playerStats);
        EventHandler.CallUpdatePlayerExpUI(GameManager.Instance.playerStats);
    }
    private void Update()
    {
        OpenMyBag();
        SwitchAnimator();
        lastAttackTime -= Time.deltaTime;
        OpenEnergyMask();
    }

    /// <summary>
    /// 注册需要监听的事件
    /// </summary>
    private void OnEnable()
    {
        EventHandler.GroundClickedEvent += OnGroundClickedEvent;
        EventHandler.EnemyClickedEvent += OnEnemyClickedEvent;
        EventHandler.EndGameEvent += OnEndGameEvent;
        EventHandler.EnemyCriticalEvent += OnEnemyCriticalEvent;
        EventHandler.PlayerDizzyEvent += OnPlayerDizzyEvent;
        GameManager.Instance.RigisterPlayer(characterStats);
    }

    /// <summary>
    /// 对象销毁时，取消事件订阅
    /// </summary>
    private void OnDisable()
    {
        EventHandler.GroundClickedEvent -= OnGroundClickedEvent;
        EventHandler.EnemyClickedEvent -= OnEnemyClickedEvent;
        EventHandler.EndGameEvent -= OnEndGameEvent;
        EventHandler.EnemyCriticalEvent -= OnEnemyCriticalEvent;
        EventHandler.PlayerDizzyEvent -= OnPlayerDizzyEvent;
    }

    /// <summary>
    /// 玩家被击退并眩晕事件
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="kicForce"></param>
    private void OnPlayerDizzyEvent(Vector3 direction, float kicForce)
    {
        if (characterStats.isImmune) return;
        agent.velocity = direction * kicForce;
        anim.SetTrigger("Dizzy");
    }

    /// <summary>
    /// 敌人暴击事件
    /// </summary>
    private void OnEnemyCriticalEvent()
    {
        if (characterStats.isImmune) return;
        anim.SetTrigger("Hit");
    }

    /// <summary>
    /// 玩家死亡，游戏结束事件
    /// </summary>
    private void OnEndGameEvent()
    {
        //执行死亡动画
        anim.SetBool("Death", true);
        isDead = true;
    }

    /// <summary>
    /// 攻击敌人事件
    /// </summary>
    /// <param name="attackTarget"></param>
    private void OnEnemyClickedEvent(GameObject attackTarget)
    {
        if (isDead) return;
        if (attackTarget != null)
        {
            this.attackTarget = attackTarget;
            characterStats.isCritical = UnityEngine.Random.value < characterStats.attackData.criticalChance;
            StartCoroutine(MoveToAttackTarget());
        }
    }

    /// <summary>
    /// 移动到目标点事件
    /// </summary>
    /// <param name="target"></param>
    private void OnGroundClickedEvent(Vector3 target)
    {
        if (isDead) return;
        StopAllCoroutines();
        agent.destination = target;
        anim.SetBool("Critical", false);
        anim.SetBool("Attack", false);
    }

    /// <summary>
    /// 移动时，根据速度选择动画（Idle、Walk、Run）
    /// </summary>
    public void SwitchAnimator()
    {
        anim.SetFloat("Speed", agent.velocity.sqrMagnitude);
    }

    /// <summary>
    /// 移动到敌人附近，等攻击冷却完毕，执行攻击动画
    /// </summary>
    /// <returns></returns>
    IEnumerator MoveToAttackTarget()
    {
        while (Vector3.Distance(attackTarget.transform.position, transform.position) > characterStats.AttackRange)
        {
            agent.destination = attackTarget.transform.position;
            yield return null;
        }
        if (lastAttackTime < 0)
        {
            transform.LookAt(attackTarget.transform);

            if (characterStats.isCritical)
            {
                anim.SetBool("Critical", true);
            }
            anim.SetBool("Attack", true);
            lastAttackTime = characterStats.CoolDown;
        }
    }

    /// <summary>
    /// 攻击动画回调事件，对敌人造成伤害
    /// </summary>
    void Hit()
    {
        if (attackTarget.CompareTag("Attackable"))//攻击石头
        {
            if (attackTarget.GetComponent<Rock>().rockStates == RockStates.HitNothing&& attackTarget.GetComponent<Rock>())
            {
                attackTarget.GetComponent<Rock>().rockStates = RockStates.HitEnemy;
                attackTarget.GetComponent<Rigidbody>().velocity = Vector3.one;
                attackTarget.GetComponent<Rigidbody>().AddForce(transform.forward * 20, ForceMode.Impulse);
            }
        }
        else//攻击敌人
        {
            var targetStats = attackTarget.GetComponent<CharacterStats>();

            targetStats.TakeDamage(characterStats, targetStats);

        }
        

    }

    /// <summary>
    /// 暴击动画回调事件
    /// </summary>
    void CriticalHit()
    {
        if (attackTarget.CompareTag("Attackable"))//攻击石头
        {
            if (attackTarget.GetComponent<Rock>().rockStates == RockStates.HitNothing && attackTarget.GetComponent<Rock>())
            {
                attackTarget.GetComponent<Rock>().rockStates = RockStates.HitEnemy;
                attackTarget.GetComponent<Rigidbody>().velocity = Vector3.one;
                attackTarget.GetComponent<Rigidbody>().AddForce(transform.forward * 20, ForceMode.Impulse);
            }
        }
        else
        {
            var targetStats = attackTarget.GetComponent<CharacterStats>();

            targetStats.TakeDamage(characterStats, targetStats);
        }
        
    }

    /// <summary>
    /// 结束动画回调事件
    /// </summary>
    void AnimationDone()
    {
        agent.destination = transform.position;
        anim.SetBool("Critical", false);
        anim.SetBool("Attack", false);
    }

    /// <summary>
    /// 打开保护罩
    /// </summary>
    void OpenEnergyMask()
    {
        energyMask.SetActive(Input.GetKey(KeyCode.E));
        characterStats.isImmune = Input.GetKey(KeyCode.E);
    }

    /// <summary>
    /// 打开背包
    /// </summary>
    void OpenMyBag()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            bool isOpen = FindObjectOfType<InventoryManager>().transform.GetChild(0).gameObject.activeInHierarchy;
            InventoryManager.Instance.OpenorCloseBag(!isOpen);
        }
    }
}
