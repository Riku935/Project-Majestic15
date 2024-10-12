using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public Image healthBar;
    public Image shieldBar;
    public float initialHealth;
    public float initialShield;
    public float healthAmount;
    public float shieldAmount;
    public float coolingTime;
    private void Start()
    {
        UIManager.obj.UpdateBar(healthAmount, shieldAmount);

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            StopCoroutine(RegenerateShield());
        }
    }
    public void TakeDamage(float damage)
    {
        StartCoroutine(RegenerateShield());
        if (shieldAmount > 0)
        {
            shieldAmount -= damage;

            if (shieldAmount < 0)
            {
                float remainingDamage = Mathf.Abs(shieldAmount); 
                shieldAmount = 0; 
                healthAmount -= remainingDamage; 
            }
        }
        else
        {
            healthAmount -= damage;
        }
        if (healthAmount <= 0)
        {
            GameManager.obj.GameOver();
        }
        UIManager.obj.UpdateBar(healthAmount, shieldAmount);
    }
    void ShieldHeal(float healingAmount)
    {
        shieldAmount += healingAmount;
        shieldAmount = Mathf.Clamp(shieldAmount, 0, initialShield);
        shieldBar.fillAmount = shieldAmount / initialShield;
    }
    IEnumerator RegenerateShield()
    {
        yield return new WaitForSeconds(coolingTime);
        ShieldHeal(20);
    }
}
