using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameState gameState = GameState.Playing;

    void Start()
    {
        gameState = GameState.StandBy;
        Debug.Log(gameState);
    }

    void Update()
    {

    }
    public void LoadNextLevel()
    {
        gameState = GameState.Playing;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        ObjectController.boxIsPlaced = false;
    }
    public void LoadCurrentLevel()
    {
        gameState = GameState.Playing;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void TapToPlay()
    {
        gameState = GameState.Playing;
        Debug.Log(gameState);
    }
}
