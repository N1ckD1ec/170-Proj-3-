using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] int health;
    int maxHealth;

    void Awake()
    {
        maxHealth = health;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if(health < 0)
            health = 0;
    }

    public void Heal(int amount)
    {
        health += amount;
        if(health > maxHealth)
            health = maxHealth;
    }
}
