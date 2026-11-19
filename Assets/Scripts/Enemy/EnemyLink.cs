using System;
using UnityEngine;

public class EnemyLink : MonoBehaviour
{
    public EnemyManager origin;
    public bool touchedRing = false;
    public int health = 3;
    public int points = 1;

    public void ReduceHealth(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ring"))
        {
            if (origin.IsHead(this))
            {
                origin.DestroyEnemyChain();
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnDestroy()
    {
        if (touchedRing)
        {
            ScoreManager.Instance.AddScore(-1);
        } else
        {
            ScoreManager.Instance.AddScore(points);
        }
    }
}
