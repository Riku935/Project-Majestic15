using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public Transform firePoint;
    public float coolDown;
    private bool canShoot = true;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canShoot == true)
        {
            Shoot();
        }
    }
    public void Shoot()
    {
        ////Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        //GameObject bullet = ProjectilePool.instance.GetPooledObject();
        //if (bullet != null)
        //{
        //    bullet.transform.position = firePoint.position;
        //    bullet.SetActive(true);
        //}
        ////StatsManager.obj.shootCount++;
        ////AudioManager.obj.playShot();
        //canShoot = false;
        //StartCoroutine(TimeShoot());
    }

    IEnumerator TimeShoot()
    {
        yield return new WaitForSeconds(coolDown);
        canShoot = true;
    }
}
