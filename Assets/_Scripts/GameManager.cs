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
    [SerializeField] private GameObject bossObject;
    [SerializeField] private GameObject gameOverMenu;

    [SerializeField] private AudioSource WinSound;

    public bool gameLost = false;
    public event Action OnHordeStarts;

    public int zombiesSpawned = 0;
   

    private void Awake()
    {
        if (instance != null) //common practice when setting up a singleton
        {
            Debug.LogWarning("Warning, it is already present another instance of the Game Manager");
        }
        instance = this; 
    }

    private void Start()
    {
        StartHorde();//1
        
        Player.instance.OnBossBattle += BossBattleBegin;//2
        
        ZombieBoss zombieBossScript = bossObject.GetComponent<ZombieBoss>();//3
        zombieBossScript.onBossDefeated += GameWon;

       Player.instance.OnGameOver += EndGame;//4

    }
    private void StartHorde()//1
    {
        OnHordeStarts?.Invoke();
    }

    private void BossBattleBegin()//2
    {
        bossObject.SetActive(true);
    }

    private void GameWon()//3
    {
        Debug.Log("game won");
        WinSound.Play();
        gameLost = true;
        gameOverMenu.SetActive(true);
        GameObject.Find("Canvas").SetActive(false);
    }

    public void EndGame()//4
    {
        Debug.Log("game lost");
        gameLost = true;
        gameOverMenu.SetActive(true);
        GameObject.Find("Canvas").SetActive(false);
    }
}
