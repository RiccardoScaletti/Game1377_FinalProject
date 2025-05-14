using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private GameObject[] ZombiePrefabs;
    private float spawnRate = 1;
    private int ZombiesToSpawn = 10;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.instance.OnHordeStarts += SpawnZombies;
    }

    private void SpawnZombies()
    {
        int zombiesToSpawn = ZombiesToSpawn * GameManager.instance.hordeNumber;
        StartCoroutine(SpawnZombiesOverTime(zombiesToSpawn, spawnRate));
    }
    private IEnumerator SpawnZombiesOverTime(int total, float interval)
    {
        for (int i = 0; i < total; i++)
        {
            Instantiate(ZombiePrefabs[Random.Range(0, 3)], transform.position, transform.rotation);
            yield return new WaitForSeconds(interval);
        }
    }

}
