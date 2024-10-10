using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 movement;     

    [SerializeField] PlayerStats stats;
    public Transform firePoint;
    public Transform tankHead;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        ProjectilePool.instance.InitializePool(stats.bulletPrefab, "Bullet");
        ProjectilePool.instance.InitializePool(stats.missilePrefab, "Missile");
    }
    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal"); 
        float moveZ = Input.GetAxisRaw("Vertical");   
        movement = new Vector3(moveX, 0f, moveZ).normalized; 

        if (Input.GetMouseButtonDown(0)) 
        {
            ShootBullet();
        }

        if (Input.GetMouseButtonDown(1) && stats.missileCount > 0) 
        {
            ShootMissile();
        }
    }

    void FixedUpdate()
    {
        Vector3 moveDirection = movement * stats.moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + moveDirection);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            Vector3 direction = (hitInfo.point - tankHead.position).normalized;
            direction.y = 0; 

            tankHead.rotation = Quaternion.LookRotation(direction);
        }
    }

    void ShootBullet()
    {
        GameObject bullet = ProjectilePool.instance.GetPooledObject("Bullet");
        if (bullet != null)
        {
            bullet.transform.position = firePoint.position;
            bullet.transform.rotation = firePoint.rotation;
            bullet.SetActive(true);
            Rigidbody rbBullet = bullet.GetComponent<Rigidbody>();
            rbBullet.velocity = firePoint.forward * stats.bulletForce;
        }
        stats.canShoot = false;
        StartCoroutine(TimeShoot());
    }
    void ShootMissile()
    {
        GameObject missile = ProjectilePool.instance.GetPooledObject("Missile");
        if (missile != null)
        {
            missile.transform.position = firePoint.position;
            missile.transform.rotation = firePoint.rotation;
            missile.SetActive(true);
            Rigidbody rbMissile = missile.GetComponent<Rigidbody>();
            rbMissile.velocity = firePoint.forward * stats.missileForce;
        }
        stats.missileCount--;
    }
    IEnumerator TimeShoot()
    {
        yield return new WaitForSeconds(stats.coolDown);
        stats.canShoot = true;
    }
    public void AddMissiles(int amount)
    {
        stats.missileCount += amount;
    }
}
