using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour
{
    [SerializeField] private AudioSource attack;
    private GameObject playerTarget;
    private NavMeshAgent agent;

    private float biteDelay = 0.5f;
    private float attackCooldown = 0f;

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

        if (attackCooldown > 0)
        {
            attackCooldown -= Time.deltaTime;
        }
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

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && attackCooldown <= 0f)
        {
            attack.Play();
            Player.instance.health -= 0.1f;
            attackCooldown = biteDelay; 
        }
        else if (other.CompareTag("Bullet"))
        {
            Player.instance.killCount++;
            Destroy(gameObject);
            Destroy(other.gameObject);
        }
    }
}
