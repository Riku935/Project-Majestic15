using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.AI;

public class Soldier : MonoBehaviour
{
    private StateMachine state;
    private Animator animator;
    private NavMeshAgent agent;
    private GameObject player;
    private bool inAttackRange;
    private float attackCount = 0;

    [Header("Enemy Settings")]
    public int soldierHealth;
    public float changeMind;
    public float rotationSpeed = 5f;
    public GameObject coin;
    public delegate void EnemyDeathDelegate();
    public event EnemyDeathDelegate OnDeath;

    [Header("Shoot Settings")]
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float bulletForce;
    public float attackRange;
    public float attackRatio;
    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
        animator = transform.GetChild(0).GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        state = GetComponent<StateMachine>();
    }
    void Start()
    {
        inAttackRange = false;
        ProjectilePool.instance.InitializePool(bulletPrefab, "EnemyBullet");
        ProjectilePool.instance.InitializePool(coin, "Coin");
        state.PushState(Idle, OnIdleEnter, null);
    }

    void Update()
    {
        inAttackRange = Vector3.Distance(transform.position, player.transform.position) < attackRange;
        LookAtPlayer();
    }

    void OnIdleEnter()
    {
        agent.ResetPath();
    }
    void Idle()
    {
        changeMind -= Time.deltaTime;
        if (changeMind<=0)
        {
            state.PushState(Chase, OnChaseEnter, OnChaseExit);
        }
    }

    void OnChaseEnter()
    {
        animator.SetBool("Chase", true);
    }
    void Chase()
    {
        agent.SetDestination(player.transform.position);
        if (inAttackRange)
        {
            state.PushState(Attack, OnEnterAttack, null);
        }
    }
    void OnChaseExit()
    {
        animator.SetBool("Chase", false);
    }

    void OnEnterAttack()
    {
        agent.ResetPath();
    }
    void Attack()
    {
        attackCount -= Time.deltaTime;
        if (!inAttackRange)
        {
            state.PopState();
        }
        else if (attackCount <= 0)
        {
            animator.SetTrigger("Attack");
            GameObject bullet = ProjectilePool.instance.GetPooledObject("EnemyBullet");
            if (bullet != null)
            {
                bullet.transform.position = firePoint.position;
                bullet.transform.rotation = firePoint.rotation;
                bullet.SetActive(true);
            }
            
            attackCount = attackRatio;
        }
    }
    void OnDeathEnter()
    {
        animator.SetBool("Dead", true);
    }
    void Death()
    {
        agent.isStopped = true;
        ScoreManager.obj.currentScore++;
        BoxCollider collider = GetComponent<BoxCollider>();
        collider.enabled = false;
        Destroy(gameObject,3f);
    }

    public void TakeDamage(int damage)
    {
        soldierHealth -= damage;
        if (soldierHealth <= 0)
        {
            if (OnDeath != null)
            {
                OnDeath.Invoke();
            }
            SpawnCoin();
            state.PushState(Death, OnDeathEnter,null);
        }
    }
    void LookAtPlayer()
    {
        Vector3 directionToPlayer = player.transform.position - transform.position;
        directionToPlayer.y = 0; 

        if (directionToPlayer != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
    void SpawnCoin()
    {
        float randomValue = Random.Range(0f, 100f);
        if (randomValue < 40f)
        {
            GameObject coin = ProjectilePool.instance.GetPooledObject("Coin");
            if (coin != null)
            {
                coin.transform.position = transform.position;
                coin.transform.rotation = transform.rotation;
                coin.SetActive(true);
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
