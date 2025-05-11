using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private Animator animator;
    
    public Transform attackPoint;
    public float attackRange = 1f;
    public int attackDamage = 20;
    public LayerMask enemyLayers;
    public float attackRate = 1f;
    float nextAttackTime = 0f;

    private const string IS_ATTACK_PARAM = "IsAttack";

    void Update()
    {
        if (Time.time >= nextAttackTime)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
        
        Debug.DrawRay(attackPoint.position, attackPoint.forward * attackRange, Color.red,3f);
    }
    
    void Attack()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyHealth>()?.TakeDamage(attackDamage);
        }
        animator.SetBool(IS_ATTACK_PARAM, true);
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

}
