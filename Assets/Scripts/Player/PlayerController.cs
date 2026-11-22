using System.Collections.Generic;
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
    Camera cam;
    Vector2 ringCenter = Vector2.zero;
    PlayerStats playerStats;
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
        playerStats = GetComponent<PlayerStats>();
        cam = Camera.main;
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
        bullet.GetComponent<Bullet>().Initilize(dir, playerStats.GetBulletDamage());
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

        
        Vector2 mousePosition = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        anglePosition = GetAngleFromMousePosition(mousePosition);
        rb.MovePosition(GetPositionOnRing(anglePosition));


        rb.MoveRotation(anglePosition * Mathf.Rad2Deg + 90f);
    }
    Vector2 GetPositionOnRing(float angle)
    {
        return radius * new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
    }
    float GetAngleFromMousePosition(Vector2 mousePos)
    {
        Vector2 dir = mousePos - ringCenter;
        return Mathf.Atan2(dir.y, dir.x);
    }
}
