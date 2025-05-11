using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    int currentHealth;

    void Start() => currentHealth = maxHealth;

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log(name + " took " + amount + " damage!");

        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        Debug.Log(name + " died!");
        Destroy(gameObject);
    }
}