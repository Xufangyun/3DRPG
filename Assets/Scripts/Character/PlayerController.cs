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
    //����Ŀ��
    private GameObject attackTarget;
    //����CD��ʱ��
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
    /// ע����Ҫ�������¼�
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
    /// ��������ʱ��ȡ���¼�����
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
    /// ��ұ����˲�ѣ���¼�
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
    /// ���˱����¼�
    /// </summary>
    private void OnEnemyCriticalEvent()
    {
        if (characterStats.isImmune) return;
        anim.SetTrigger("Hit");
    }

    /// <summary>
    /// �����������Ϸ�����¼�
    /// </summary>
    private void OnEndGameEvent()
    {
        //ִ����������
        anim.SetBool("Death", true);
        isDead = true;
    }

    /// <summary>
    /// ���������¼�
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
    /// �ƶ���Ŀ����¼�
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
    /// �ƶ�ʱ�������ٶ�ѡ�񶯻���Idle��Walk��Run��
    /// </summary>
    public void SwitchAnimator()
    {
        anim.SetFloat("Speed", agent.velocity.sqrMagnitude);
    }

    /// <summary>
    /// �ƶ������˸������ȹ�����ȴ��ϣ�ִ�й�������
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
    /// ���������ص��¼����Ե�������˺�
    /// </summary>
    void Hit()
    {
        if (attackTarget.CompareTag("Attackable"))//����ʯͷ
        {
            if (attackTarget.GetComponent<Rock>().rockStates == RockStates.HitNothing&& attackTarget.GetComponent<Rock>())
            {
                attackTarget.GetComponent<Rock>().rockStates = RockStates.HitEnemy;
                attackTarget.GetComponent<Rigidbody>().velocity = Vector3.one;
                attackTarget.GetComponent<Rigidbody>().AddForce(transform.forward * 20, ForceMode.Impulse);
            }
        }
        else//��������
        {
            var targetStats = attackTarget.GetComponent<CharacterStats>();

            targetStats.TakeDamage(characterStats, targetStats);

        }
        

    }

    /// <summary>
    /// ���������ص��¼�
    /// </summary>
    void CriticalHit()
    {
        if (attackTarget.CompareTag("Attackable"))//����ʯͷ
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
    /// ���������ص��¼�
    /// </summary>
    void AnimationDone()
    {
        agent.destination = transform.position;
        anim.SetBool("Critical", false);
        anim.SetBool("Attack", false);
    }

    /// <summary>
    /// �򿪱�����
    /// </summary>
    void OpenEnergyMask()
    {
        energyMask.SetActive(Input.GetKey(KeyCode.E));
        characterStats.isImmune = Input.GetKey(KeyCode.E);
    }

    /// <summary>
    /// �򿪱���
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
