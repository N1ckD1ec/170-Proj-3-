using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float startingAngle = 270;
    [SerializeField] float fireRate = 0.2f;
    
    [SerializeField] RingGenerator ringGenerator;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject firePoint;
    Rigidbody2D rb;
    float radius;
    float anglePosition;
    float moveInput;
    bool fireHeld;
    float nextFireTime;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        radius = ringGenerator.radius;
        anglePosition = startingAngle * Mathf.Deg2Rad;
        rb.position = GetPositionOnRing(anglePosition);
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<float>();
    }
    public void OnShoot(InputAction.CallbackContext context)
    {
        if(GameManager.Instance.gameState != GameState.PLAY)
            return;
        fireHeld = context.ReadValueAsButton();
    }
    void fireBullet()
    {
        Vector2 dir = transform.up;
        GameObject bullet = Instantiate(bulletPrefab, firePoint.transform.position, Quaternion.identity);
        bullet.GetComponent<Bullet>().Initilize(dir);
    }
    void Update()
    {
        if(GameManager.Instance.gameState != GameState.PLAY) return;
        if(!fireHeld || Time.time < nextFireTime) return;
        fireBullet();
        nextFireTime = Time.time + fireRate;
    }
    void FixedUpdate()
    {
        if(GameManager.Instance.gameState != GameState.PLAY)
            return;
        anglePosition += moveInput * (moveSpeed / radius) * Time.fixedDeltaTime;
        anglePosition = anglePosition % (2 * Mathf.PI);
        rb.MovePosition(GetPositionOnRing(anglePosition));
        rb.MoveRotation(anglePosition * Mathf.Rad2Deg + 90f); //This line could change as we want to shoot...
    }
    Vector2 GetPositionOnRing(float angle)
    {
        return radius * new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
    }
}
