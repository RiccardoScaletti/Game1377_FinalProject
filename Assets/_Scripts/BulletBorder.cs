using UnityEngine;

public class BulletBorder : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bullet" || other.gameObject.tag == "Fireball")
        {
            Destroy(other.gameObject);
        }
    }
}
