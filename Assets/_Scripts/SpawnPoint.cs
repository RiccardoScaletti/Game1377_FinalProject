using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private GameObject[] ZombiePrefabs;
    private float spawnRate = 0.5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.instance.OnHordeStarts += SpawnZombies;
    }
  
    private void SpawnZombies()
    {
        StartCoroutine(SpawnZombiesOverTime(spawnRate));
    }
    private IEnumerator SpawnZombiesOverTime(float interval)
    {
        for (int i = 0; i < 25; i++)
        {
            Instantiate(ZombiePrefabs[Random.Range(0, 3)], transform.position, transform.rotation);
            GameManager.instance.zombiesSpawned++;
            yield return new WaitForSeconds(interval);
        }
    }
}
