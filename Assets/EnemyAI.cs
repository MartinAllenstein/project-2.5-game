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
    
    
    public Transform attackPoint;                // จุดที่ใช้ตรวจสอบการโจมตี
    public Vector3 attackBoxSize = new Vector3(1f, 1f, 1f); // ขนาด hitbox
    public float attackWarningTime = 0.5f;       // เวลาแสดงพื้นที่สีแดงก่อนโจมตี
    public GameObject attackWarningPrefab;       // พื้นที่สีแดงแจ้งเตือน
    private BoxCollider attackCollider;
    
    private GameObject attackWarningInstance;    // อินสแตนซ์ของพื้นที่สีแดง
    
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
        
        // อ่าน Collider จาก attackPoint
        attackCollider = attackPoint.GetComponent<BoxCollider>();
        if (attackCollider != null)
        {
            attackBoxSize = attackCollider.size;
        }
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRadius)
        {
            // ถ้าผู้เล่นอยู่ในระยะ ตรวจสอบการโจมตี
            // if (!isAttacking)
            // {
            //     StartCoroutine(AttackPlayer());
            // }
            // หยุดอนิเมชันเดินเมื่อเข้าสู่โหมดโจมตี
            //animator.SetBool("IsWalk", false);
            
            // เดินหา player
            ChasePlayer();

            // เริ่มโจมตีถ้าเข้าใกล้พอ และยังไม่โจมตี
            if (!isAttacking && distanceToPlayer < attackBoxSize.x)
            {
                StartCoroutine(AttackPlayer());
            }
        }
        else
        {
            Patrol(); // เดินวนถ้าไม่มีผู้เล่นอยู่ใกล้
        }
    }

    void Patrol()
    {
        if (isAttacking) return; // ไม่ให้เดินถ้ากำลังโจมตี
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
        
        animator.SetBool("IsWalk", false);
        
        // ตำแหน่งแสดงเอฟเฟกต์เตือน → ชิดพื้น BoxCollider
        Vector3 warningPosition = attackPoint.position - new Vector3(0, attackBoxSize.y - attackBoxSize.y, 0);

        // แสดงพื้นที่เตือนก่อนโจมตี
        if (attackWarningPrefab != null)
        {
            attackWarningInstance = Instantiate(attackWarningPrefab, warningPosition, attackPoint.rotation, transform);

            // ตั้งขนาดให้เท่ากับ attackBoxSize
            attackWarningInstance.transform.localScale = new Vector3(attackBoxSize.x, attackBoxSize.y, attackBoxSize.z);
        }


        yield return new WaitForSeconds(attackWarningTime);

        if (attackWarningInstance != null)
        {
            Destroy(attackWarningInstance);
        }
        
        // ตรวจสอบผู้เล่นภายใน hitbox
        Collider[] hits = Physics.OverlapBox(attackPoint.position, attackBoxSize, attackPoint.rotation);
        foreach (Collider hit in hits)
        {
            //if (hit.CompareTag("Player"))
            
                // ตรวจว่า player มีคอมโพเนนต์ Health แล้วส่งดาเมจ (ต้องมี PlayerControls script)
                hit.GetComponent<PlayerControls>()?.TakeDamage(attackDamage);
                Debug.Log("Enemy hit player with attack box!");
                animator.SetTrigger("Attack");
            
        }
        
        yield return new WaitForSeconds(attackCooldown);
        
        isAttacking = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(initialPosition == Vector3.zero ? transform.position : initialPosition, patrolRadius);
        
        // แสดง hitbox
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(attackPoint.position, attackBoxSize);
        }
    }
    
    void ChasePlayer()
    {
        if (isAttacking) return; // ไม่ให้เดินถ้ากำลังโจมตี
        
        Vector3 direction = (player.position - transform.position).normalized;
        
        // ปรับการเคลื่อนที่
        Vector3 move = direction * patrolSpeed * Time.deltaTime;
        move.y = 0; // ล็อคแกน Y ไม่ให้ enemy จม
        
        transform.position += move;

        // หมุนไปในทิศทางที่เดิน (สำหรับ sprite 2D หันซ้าย-ขวา)
        if (direction.x != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Sign(direction.x) * Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
        
        animator.SetBool("IsWalk", true);
        
    }

}
