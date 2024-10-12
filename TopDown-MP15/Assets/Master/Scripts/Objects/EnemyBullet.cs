using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public int bulletDamage;
    public int bulletForce;
    public GameObject impactEffect;
    private Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void OnEnable()
    {
        float exitTime = 10;
        StartCoroutine(SetOff(exitTime));
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null) { playerHealth.TakeDamage(bulletDamage); }
            BulletImpact();
            gameObject.SetActive(false);
        }
        
    }
    private void Update()
    {
        BulletMovement();
    }
    void BulletMovement()
    {
        rb.velocity = transform.forward * bulletForce;
    }
    void BulletImpact()
    {
        Instantiate(impactEffect, transform.position, transform.rotation);
    }
    IEnumerator SetOff(float exitTime)
    {
        yield return new WaitForSeconds(exitTime);
        gameObject.SetActive(false);
    }
}
