using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject instructionsPanel;
    public void ShowInstructions()
    {
        instructionsPanel.SetActive(true);
    }
    public void HideInstructions()
    {
        instructionsPanel.SetActive(false);
    }
    public void Reload()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainScene");
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
