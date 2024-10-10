using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager obj;
    public bool gameReady;

    private void Awake()
    {
        obj = this;
    }
    public void GameOver()
    {

    }
    private void OnDestroy()
    {
        obj = null;
    }
}
