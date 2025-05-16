using UnityEngine;

public class DropsSpawnManager : MonoBehaviour
{

    [SerializeField]private GameObject[] spawnables;
   
    public void SpawnUsableObject(int index, Vector3 position, Quaternion rotation)
    {
        Debug.LogWarning("SPAWNED");
        Instantiate(spawnables[index], new Vector3(position.x, position.y +2f, position.z), rotation);
    }
}
