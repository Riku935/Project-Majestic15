using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public int bulletDamage;
    public int bulletForce;
    public GameObject impactEffect;
    private Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        ProjectilePool.instance.InitializePool(impactEffect, "ImpactEffect");
    }
    private void OnEnable()
    {
        float exitTime = 10;
        StartCoroutine(SetOff(exitTime));
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Soldier enemy = collision.gameObject.GetComponent<Soldier>();
            if (enemy != null)
            {
                enemy.TakeDamage(bulletDamage);
            }
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
        GameObject impact = ProjectilePool.instance.GetPooledObject("ImpactEffect");
        if (impact != null)
        {
            impact.transform.position = transform.position;
            impact.transform.rotation = transform.rotation;
            impact.SetActive(true);
        }
    }
    IEnumerator SetOff(float exitTime)
    {
        yield return new WaitForSeconds(exitTime);
        gameObject.SetActive(false);
    }
}
