using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    public float patrolRadius = 3f;            // รัศมีที่เดินวนอยู่
    public float patrolSpeed = 2f;             // ความเร็วในการเดิน
    public float detectionRadius = 5f;         // ระยะตรวจจับผู้เล่น
    public float attackCooldown = 2f;          // เวลาคูลดาวน์ระหว่างโจมตี
    public int attackDamage = 10;              // ความเสียหายจากการโจมตี
    public float waitTimeAtPoint = 2f;           // เวลาหยุดรอเมื่อเดินถึงจุด
    
    private Vector3 initialPosition;             // ตำแหน่งเริ่มต้นสำหรับสุ่มเดิน
    private Vector3 patrolTarget;                // ตำแหน่งเป้าหมายที่เดินไป
    private bool isWaiting = false;              // รอเวลาอยู่ ณ จุดสุ่ม
    private float waitTimer = 0f;
    private bool hasTarget = false;

    private bool isAttacking = false;            // ป้องกันการโจมตีซ้ำ
    private Transform player;                    // ผู้เล่น

    private Animator animator;
    
    void Start()
    {
        initialPosition = transform.position;
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRadius)
        {
            // ถ้าผู้เล่นอยู่ในระยะ ตรวจสอบการโจมตี
            if (!isAttacking)
            {
                StartCoroutine(AttackPlayer());
            }
            // หยุดอนิเมชันเดินเมื่อเข้าสู่โหมดโจมตี
            animator.SetBool("IsWalk", false);
        }
        else
        {
            Patrol(); // เดินวนถ้าไม่มีผู้เล่นอยู่ใกล้
        }
    }

    void Patrol()
    {
        if (!hasTarget || Vector3.Distance(transform.position, patrolTarget) < 0.1f)
        {
            if (!isWaiting)
            {
                isWaiting = true;
                waitTimer = waitTimeAtPoint;
                animator.SetBool("IsWalk", false);
            }

            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0f)
            {
                // เลือกตำแหน่งใหม่แบบสุ่มภายในรัศมี
                Vector2 randomCircle = Random.insideUnitCircle * patrolRadius;
                patrolTarget = initialPosition + new Vector3(randomCircle.x, 0, randomCircle.y);
                hasTarget = true;
                isWaiting = false;
            }
            return;
        }

        // เดินไปยังจุดเป้าหมายใหม่
        Vector3 direction = (patrolTarget - transform.position).normalized;
        transform.position += direction * patrolSpeed * Time.deltaTime;
        
        animator.SetBool("IsWalk", true);

        // หมุนไปในทิศทางที่เดิน (สำหรับ sprite 2D หันซ้าย-ขวา)
        if (direction.x != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Sign(direction.x) * Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
    }
    
    IEnumerator AttackPlayer()
    {
        isAttacking = true;

        animator.SetTrigger("Attack");

        // ตรวจว่า player มีคอมโพเนนต์ Health แล้วส่งดาเมจ (คุณต้องมี PlayerHealth script)
        player.GetComponent<PlayerControls>()?.TakeDamage(attackDamage);

        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(initialPosition == Vector3.zero ? transform.position : initialPosition, patrolRadius);
    }
}
