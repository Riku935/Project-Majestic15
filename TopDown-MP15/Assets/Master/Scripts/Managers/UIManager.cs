using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager obj;
    public TMP_Text score;
    public TMP_Text missileText;
    public TMP_Text waveText;
    public TMP_Text enemiesText;
    public TMP_Text coinText;
    public TMP_Text coinTextShop;
    public TMP_Text shieldText;
    public TMP_Text HealthText;
    public Image healthBar;
    public Image shieldBar;
    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject mainGamePanel;
    [SerializeField] GameObject storePanel;

    private void Awake()
    {
        obj = this;
    }
    private void Update()
    {
        updateScore();
        updateWave();
    }

    public void updateScore()
    {
        score.text = "Score: " + ScoreManager.obj.currentScore;
    }
    public void updateWave()
    {
        WaveManager waveManager = FindObjectOfType<WaveManager>();
        if (waveManager != null)
        {
            waveText.text = "Wave: " + (waveManager.currentWaveIndex + 1);
            enemiesText.text = "Enemies: " + waveManager.enemiesAlive;
        }
    }
    public void UpdateCoins()
    {
        coinText.text = "" + GameManager.obj.coinCount;
        coinTextShop.text = "Coins: " + GameManager.obj.coinCount;
    }
    public void UpdateMissile(int missileCount)
    {
        missileText.text = "" + missileCount;
    }
    public void UpdateBar(float healthAmount, float shieldAmount)
    {
        PlayerHealth player = FindObjectOfType<PlayerHealth>();
        healthBar.fillAmount = healthAmount / 60f;
        shieldBar.fillAmount = shieldAmount / player.initialShield;
        shieldText.text = shieldAmount.ToString();
        HealthText.text = healthAmount.ToString();
    }
    public void Pause()
    {
        pausePanel.SetActive(true);
        GameManager.obj.PauseGame();
    }
    public void Resume()
    {
        pausePanel.SetActive(false);
        GameManager.obj.ContinueGame();
    }

    public void GameOver()
    {
        mainGamePanel.SetActive(false);
        gameOverPanel.SetActive(true);
    }
    public void Store()
    {
        storePanel.SetActive(true);
    }
    public void ResumeStore()
    {
        storePanel.SetActive(false);
        GameManager.obj.ResumeFromShop();
    }
    public void Exit()
    {
        GameManager.obj.ExitGame();
    }
    public void Reload()
    {
        GameManager.obj.Reload();
    }

    public void BuyHealth()
    {
        GameManager.obj.BuyHealth();

    }
    public void BuyShield()
    {
        GameManager.obj.BuyShield();

    }

    public void BuyMissile()
    {
        GameManager.obj.BuyMissile();

    }
    private void OnDestroy()
    {
        obj = null;
    }
}
