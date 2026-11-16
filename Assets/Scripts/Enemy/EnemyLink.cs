using System;
using UnityEngine;

public class EnemyLink : MonoBehaviour
{
    public EnemyManager origin;
    public static event Action<int> OnEnemyTouchedRing;
    public bool touchedRing = false;

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
            OnEnemyTouchedRing?.Invoke(-1);
        }
    }
}
