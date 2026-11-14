using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] int health;
    int maxHealth;

    void Awake()
    {
        maxHealth = health;
    }

    void OnEnable()  => GameManager.StateChanged += OnStateChanged;
    void OnDisable() => GameManager.StateChanged -= OnStateChanged;

    void OnStateChanged(GameState previous, GameState next)
    {
        if(next == GameState.PLAY && previous != GameState.PAUSED)
            health = maxHealth;
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
