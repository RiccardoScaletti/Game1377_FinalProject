using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    private int level = 0;
    private int playerLife = 3;

    //store gameobjects for the player and rival to target scripts that plays animations
    [SerializeField] private GameObject playerCharacter;

    private void Awake()
    {
        if (instance != null) //common practice when setting up a singleton
        {
            Debug.LogWarning("Warning, it is already present another instance of the Game Manager");
        }
        instance = this; // instance initialization, needed to define a singleton
        //assigns the variable instance to itself
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        if (playerLife <= 0)
        {
            EndGame();
        }
    }

    private void EndGame()
    {
        Debug.Log("game lost");
        Application.Quit();
    }
}
