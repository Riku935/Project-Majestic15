using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.AI;

public class Soldier : MonoBehaviour
{
    private StateMachine state;
    private Animator animator;
    private UnityEngine.AI.NavMeshAgent agent;
    private GameObject player;
    private bool inAttackRange;
    private float attackCount;

    public int soldierHealth;
    public float changeMind;
    public float attackRange;
    public float attackRatio;
    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
        animator = transform.GetChild(0).GetComponent<Animator>();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        state = GetComponent<StateMachine>();
    }
    void Start()
    {
        inAttackRange = false;
        attackCount = attackRatio;
        state.PushState(Idle, OnIdleEnter, null);
    }

    void Update()
    {
        inAttackRange = Vector3.Distance(transform.position, player.transform.position) < attackRange;
        if (soldierHealth <= 0) { state.PushState(Death,OnDeathEnter,OnDeathExit); }
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
            //player.Hurt(2, 1);
            attackCount = attackRatio;
        }
    }
    void OnDeathEnter()
    {
        animator.SetBool("Death", true);
    }
    void Death()
    {
        
    }
    void OnDeathExit()
    {
        
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
