using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager obj;
    public bool gameReady;
    public bool gamePaused;
    public int coinCount;
    private void Awake()
    {
        obj = this;
    }
    public void PauseGame()
    {
        if (!gamePaused)
        {
            gamePaused = true;
            Time.timeScale = 0f; 
        }
    }

    public void ContinueGame()
    {
        if (gamePaused)
        {
            gamePaused = false;
            Time.timeScale = 1f; 
        }
    }

    public void TimeBetweenRounds()
    {
        if (!gamePaused)
        {
            gamePaused = true;
            Time.timeScale = 0f; 
            UIManager.obj.Store();
        }
    }
    public void ResumeFromShop()
    {
        if (gamePaused)
        {
            gamePaused = false;
            Time.timeScale = 1f; 
           
            WaveManager objWaveManager = FindObjectOfType<WaveManager>();
            if (objWaveManager != null)
            {
                objWaveManager.ResumeWave();
            }
        }
    }
    public void BuyHealth()
    {
        if (coinCount >= 2)
        {
            coinCount -=2;
            PlayerHealth player = FindObjectOfType<PlayerHealth>();
            player.healthAmount = player.initialHealth;
            UIManager.obj.UpdateBar(player.initialHealth, player.shieldAmount);
        }
    }
    public void BuyShield()
    {
        if (coinCount >= 2)
        {
            coinCount -=2;
            PlayerHealth player = FindObjectOfType<PlayerHealth>();
            player.initialShield += 50;
            UIManager.obj.UpdateBar(player.healthAmount, player.initialShield);
        }
    }

    public void BuyMissile()
    {
        if (coinCount >= 1)
        {
            coinCount -=1;
            PlayerController player = FindObjectOfType<PlayerController>();
            player.stats.missileCount++;
            UIManager.obj.UpdateMissile(player.stats.missileCount);
        }
        
    }
    public void SetCoin()
    {
        coinCount++;
        UIManager.obj.UpdateCoins();
    }
    public void Reload()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainScene");
    }
    public void GameOver()
    {
        UIManager.obj.GameOver();
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    private void OnDestroy()
    {
        obj = null;
    }
}
