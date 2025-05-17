using UnityEngine;

public class SpawnableData : MonoBehaviour
{
    [SerializeField]private WeaponData weapon;
    private float despawnTimer = 0;

    private void Update()
    {
        transform.Rotate(0f, 0f, 70 * Time.deltaTime);
        despawnTimer += Time.deltaTime;
        if (despawnTimer >= 20) Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Player.instance.pickUpAudioSource.Play();
            Player.instance.EquipWeapon(weapon.name);
            Destroy(gameObject); 
        }
        
    }
}
