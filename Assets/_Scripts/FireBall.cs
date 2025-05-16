using UnityEngine;

public class FireBall : MonoBehaviour
{
   
    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * 100 * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") Player.instance.PlayerWound(0.2f);
    }
}
