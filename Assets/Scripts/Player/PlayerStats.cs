using UnityEngine;
using System.Collections;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] int health = 3;
    [SerializeField] int damage = 1;
    [SerializeField] private SpriteRenderer playerSprite;
    [SerializeField] private Color damageColor = Color.red;
    [SerializeField] private float flashDuration = 0.15f;

    private Color originalColor;
    private Coroutine flashCoroutine;
    int maxHealth;

    void Awake()
    {
        maxHealth = health;
        if (playerSprite == null)
            playerSprite = GetComponentInChildren<SpriteRenderer>();
        originalColor = playerSprite.color;
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
        ScoreManager.Instance.AddScore(-1);
        health -= damage;
        if(health < 0)
            health = 0;
        Debug.Log(health);

        if (flashCoroutine != null)
            StopCoroutine(flashCoroutine);
        flashCoroutine = StartCoroutine(DamageFlash());
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

    private IEnumerator DamageFlash()
    {
        playerSprite.color = damageColor;

        yield return new WaitForSeconds(flashDuration);

        playerSprite.color = originalColor;
        flashCoroutine = null;
    }
}
