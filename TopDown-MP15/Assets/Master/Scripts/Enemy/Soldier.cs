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
    private float changeMind;


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
        state.PushState(Idle, OnIdleEnter, null);
    }

    void Update()
    {
        inAttackRange = Vector3.Distance(transform.position, player.transform.position) < attackRange;
    }

    void OnIdleEnter()
    {
        agent.ResetPath();
    }
    void Idle()
    {
        changeMind--;
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
        if (Vector3.Distance(transform.position, player.transform.position) > 5.5f)
        {
            state.PopState();
            state.PushState(Idle, OnIdleEnter, null);
        }
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
        attackRatio -= Time.deltaTime;
        if (!inAttackRange)
        {
            state.PopState();
        }
        else if (attackRatio <= 0)
        {
            animator.SetTrigger("Attack");
            //player.Hurt(2, 1);
            attackRatio = 2f;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
