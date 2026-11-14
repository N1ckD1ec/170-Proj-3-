using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float speed = 10f;
    [SerializeField] int damage = 1;
    [SerializeField] float backupLifetime;
    public void Initilize(Vector2 direction)
    {
        GetComponent<Rigidbody2D>().linearVelocity = direction.normalized * speed;
    }
    public void Start()
    {
        Destroy(gameObject, backupLifetime);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        // if(collision.CompareTag("Enemy"))
        // {

        // }
        Destroy(gameObject);
    }
}
