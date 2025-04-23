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
    
    public int hordeNumber = 1;

    public event Action OnHordeStarts;


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
        StartHorde();
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

    private void EndGame()
    {
        Debug.Log("game lost");
        Application.Quit();
    }
}
