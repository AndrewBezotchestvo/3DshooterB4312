using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [Header("EnemySettings")]
    [SerializeField] private float maxHP = 100f;
    [SerializeField] private float damage = 30f;


    [Header("Chase Settings")]
    [SerializeField] private float chaseSpeed = 4f;
    [SerializeField] private float defoultChaseRange = 10f;
    [SerializeField] private float folowingChaseRange = 20f;
    [SerializeField] private float attackRange = 2f;

    [Header("References")]
    [SerializeField] private Transform player;

    private NavMeshAgent agent;
    private bool isWaiting;
    private bool isChasing;
    private bool isAttacking;
    private Animator animator;
    private float chaseRange;
    private float hp;

    // Animation parameters
    private readonly int animSpeed = Animator.StringToHash("Speed");
    private readonly int animReaction = Animator.StringToHash("Reaction");
    private readonly int animDie = Animator.StringToHash("Death");

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
        animator = GetComponent<Animator>();
        chaseRange = defoultChaseRange;
        hp = maxHP;
    }

    void Update()
    {
        if (hp <= 0) return;
        if (isAttacking) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            Attack();
            return;
        }

        if (distanceToPlayer <= chaseRange)
        {
            chaseRange = folowingChaseRange;
            ChasePlayer();
            isChasing = true;
        }
        else if (isChasing)
        {
            isChasing = false;
            chaseRange = defoultChaseRange;
        }

        if (!isChasing && !isWaiting && agent.remainingDistance < 0.5f)
        {
            StartWaiting();
        }

        if (isWaiting)
        {

        }

        UpdateAnimations();
    }


    void StartWaiting()
    {
        isWaiting = true;
    }

    void ChasePlayer()
    {
        agent.speed = chaseSpeed;
        agent.destination = player.position;
    }

    void Attack()
    {
        isAttacking = true;
        agent.isStopped = true;

        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);

        //animator.SetTrigger(animAttack);

        //player.GetComponent<PlayerController>().GetDamage(TakeDamage());
        // ����� ����� ���������� �������������
        Invoke(nameof(ResetAttack), 0.5f); // ������������, ��� �������� ����� ������ 1 �������
    }

    void ResetAttack()
    {
        isAttacking = false;
        agent.isStopped = false;
    }

    void UpdateAnimations()
    {
        float speed = agent.velocity.magnitude;
        animator.SetFloat(animSpeed, Mathf.Clamp01(speed/4f));

        bool isMoving = speed > 0.1f;
        //animator.SetBool(animIsMoving, isMoving);

        // ����� �������� ������ ������ ��� ������ �������� �����
    }


    public void GetDamage(float damage)
    {
        hp -= damage;

        if (animator.GetCurrentAnimatorStateInfo(0).shortNameHash == animDie) return;
        
        if (hp <= 0) Die();
        else animator.SetTrigger(animReaction); 
    }

    float TakeDamage()
    {
        return Random.Range(damage/10, damage);
    }

    // ������������ � ���������
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    void Die()
    {
        animator.SetTrigger(animDie);
        Invoke("DestroyEnemy", 5); //pапустить функцию DestroyEnemy через 5 секунд
    }

    void DestroyEnemy()
    {
        Destroy(gameObject); //уничтожить объект
    }
}

