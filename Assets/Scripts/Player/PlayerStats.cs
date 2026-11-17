using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] int health = 3;
    [SerializeField] int damage = 1;
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
    public int GetBulletDamage()
    {
        return damage;
    }
    public void SetBulletDamage(int newDamage)
    {
        damage = newDamage;
    }
    public void ResetBulletDamage()
    {
        damage = 1;
    }
}
