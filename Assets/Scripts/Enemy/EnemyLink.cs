using System;
using UnityEngine;

public class EnemyLink : MonoBehaviour
{
    public EnemyManager origin;
    public bool touchedRing = false;
    public int health = 3;

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
            origin.DestroyEnemyChain();
        }
    }

    private void OnDestroy()
    {
        if (touchedRing)
        {
            ScoreManager.Instance.AddScore(-1);
        } else
        {
            int points = 1;
            if (origin.IsHead(this))
            {
                points = 5;
            }
            ScoreManager.Instance.AddScore(points);
        }
    }
}
