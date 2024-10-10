using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Scriptable Objects/Player")]
public class PlayerStats : ScriptableObject
{
    public float moveSpeed;
    public int health;

    [Header("Shoot Settings")]
    public GameObject bulletPrefab;
    public GameObject missilePrefab;
    public bool canShoot = true;
    public float coolDown;
    public float bulletForce;
    public float missileForce;
    public int missileCount;
}
