using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    //store gameobjects for the player and rival to target scripts that plays animations
    [SerializeField] private GameObject playerCharacter;
    
    public event Action OnHordeStarts;

    public int zombiesSpawned = 0;
   

    private void Awake()
    {
        if (instance != null) //common practice when setting up a singleton
        {
            Debug.LogWarning("Warning, it is already present another instance of the Game Manager");
        }
        instance = this; // instance initialization, needed to define a singleton
    }

    private void Start()
    {
        StartHorde();
        ZombieBoss.instance.onBossDefeated += GameWon;
    }

    private void Update()
    {
        if (Player.instance.health <= 0)
        {
            EndGame();
        }
        
    }

    private void StartHorde()
    {
        OnHordeStarts?.Invoke();
    }
    private void GameWon()
    {
        Debug.Log("game won");
        Application.Quit();
    }

    private void EndGame()
    {
        Debug.Log("game lost");
        Application.Quit();
    }
}
