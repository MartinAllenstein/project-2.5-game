using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private Animator animator;
    
    //Animation Parameters
    private const string IS_HURT_PARAM = "Hurt";
    private const string IS_DEAD_PARAM = "IsDead";
    
    public int maxHealth = 100;
    int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        //LookAtPlayer();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        
        animator.SetTrigger(IS_HURT_PARAM);
        
        Debug.Log(name + " took " + damage + " damage!");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        animator.SetBool(IS_DEAD_PARAM,true);
        
        Debug.Log(name + " died!");
        
        GetComponent<Collider>().enabled = false;
        this.enabled = false;
        
        //Destroy(gameObject);
    }

    // [SerializeField] private SpriteRenderer enemySprite;
    // public Transform player;
    // public bool isFlipped = false;
    
    //Flip Enemy
    // public void LookAtPlayer()
    // {
    //     Vector3 flipped = transform.localScale;
    //     flipped.x *= -1;
    //     if (transform.position.x > player.position.x && enemySprite)
    //     {
    //         enemySprite.flipX = true;
    //     }
    //     else if (transform.position.x < player.position.x && !enemySprite)
    //     {
    //         enemySprite.flipX = false;
    //     }
    // }
    
    // if (x != 0 && x != 0)
    // {
    //     enemySprite.flipX = true;
    // }
    //
    // if (x != 0 && x > 0)
    // {
    //     enemySprite.flipX = false;
    // }
}