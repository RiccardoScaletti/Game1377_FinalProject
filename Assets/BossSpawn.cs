using UnityEngine;

public class BossSpawn : MonoBehaviour
{

    [SerializeField] private GameObject bossPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Player.instance.OnBossBattle += SpawnBoss;
    }

    private void SpawnBoss()
    {
        Instantiate(bossPrefab, transform.position, transform.rotation);
    }

}
