using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    public int missileForce;

    public float explosionRadius = 5f;     
    public float explosionForce = 700f;    
    public int missileDamage = 50;         
    public LayerMask enemyLayer;


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
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Obstacle")
        {
            Explode();
        }
    }
    private void Update()
    {
        MissileMovement();
    }
    void Explode()
    {
        if (impactEffect != null)
        {
            Instantiate(impactEffect, transform.position, transform.rotation);
        }

        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius, enemyLayer);

        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }
            Soldier enemy = nearbyObject.GetComponent<Soldier>();
            if (enemy != null)
            {
                enemy.TakeDamage(missileDamage);
            }
        }
        gameObject.SetActive(false);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
    void MissileMovement()
    {
        rb.velocity = transform.forward * missileForce;
    }
    IEnumerator SetOff(float exitTime)
    {
        yield return new WaitForSeconds(exitTime);
        gameObject.SetActive(false);
    }
}
