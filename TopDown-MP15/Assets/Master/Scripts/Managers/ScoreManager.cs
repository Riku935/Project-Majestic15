using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager obj;
    public int currentScore = 0;

    private void Awake()
    {
        obj = this;
    }
    public void UpdateScore(int cantidad) //Metodo super inecesario pero bueno
    {
        currentScore += cantidad;
    }
    private void OnDestroy()
    {
        obj = null;
    }
}
