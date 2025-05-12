using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private Animator animator;
    
    //Attack
    public Transform attackPoint; 
    public LayerMask enemyLayers;
    
    public int attackDamage = 20; 
    public float attackRange = 1f;
    
    public float attackRate = 1f;
    float nextAttackTime = 0f;
    
    //Animation Parameters
    private const string IS_ATTACK_PARAM = "Attack";

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
        
        //Debug.DrawRay(attackPoint.position, attackPoint.forward * attackRange, Color.red,3f);
    }
    
    void Attack()
    {
        //Detect enemies in range
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers);
        
        //Damage
        foreach (Collider enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyHealth>()?.TakeDamage(attackDamage);
        }
        animator.SetTrigger(IS_ATTACK_PARAM);
    }

    //Draw range
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

}
