using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] float speed = 2f;
    [SerializeField] float backupLifetime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, backupLifetime);
    }

    public void Initilize(Vector3 direction)
    {
        GetComponent<Rigidbody2D>().linearVelocity = direction.normalized * speed;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerStats>().TakeDamage(1);
            Destroy(gameObject);
        }
    }
}
