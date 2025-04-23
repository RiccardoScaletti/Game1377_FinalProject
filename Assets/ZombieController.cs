using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour
{
    [SerializeField] private AudioSource attack;
    private GameObject playerTarget;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (playerTarget == null)
            playerTarget = GameObject.FindGameObjectWithTag("Player");       
    }

    void Update()
    {
        if (playerTarget == null )
            return;
        agent.SetDestination(playerTarget.transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            attack.Play();
            Player.instance.health -= 0.1f;
        }
        else if (other.tag == "Bullet")
        {
            Player.instance.killCount++;
            Destroy(gameObject);
            Destroy(other.gameObject);
        }
    }
}
