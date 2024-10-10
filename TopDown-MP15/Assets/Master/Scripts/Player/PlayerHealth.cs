using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public Image healthBar;
    public Image shieldBar;
    public float healthAmount;
    public float shieldAmount;
    public float coolingTime;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            TakeDamage(10);
        }
    }
    void TakeDamage(float damage)
    {
        if (shieldAmount > 0)
        {
            shieldAmount -= damage;
            shieldBar.fillAmount = shieldAmount / 100f;
        }
        if (shieldAmount <= 0 )
        {
            healthAmount -= damage;
            healthBar.fillAmount = healthAmount / 100f;
        }
        
    }
    void ShieldHeal(float healingAmount)
    {
        shieldAmount += healingAmount;
        shieldAmount = Mathf.Clamp(shieldAmount, 0, 100);
        shieldBar.fillAmount = shieldAmount / 100;
    }
    IEnumerator RegenerateShield()
    {
        yield return new WaitForSeconds(coolingTime);
        ShieldHeal(20);
    }
}
