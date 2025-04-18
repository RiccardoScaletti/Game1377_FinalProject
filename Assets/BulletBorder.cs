using UnityEngine;

public class BulletBorder : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            Debug.Log("Bullet");
            Destroy(other.gameObject);
        }
    }
}
