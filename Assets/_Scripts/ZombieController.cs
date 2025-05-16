using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour
{
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private AudioClip[] deathSounds;

    [SerializeField] private AudioSource zombiesound;

    [SerializeField] private GameObject ZombieMapIndicator;

    private Animator animator;

    private GameObject playerTarget;
    private NavMeshAgent agent;

    private float biteDelay = 0.5f;
    private float attackCooldown = 0f;
    private float despawnCooldown = 0f;
    bool isDead = false;

    private int rndSpawnChance;
    private DropsSpawnManager dropsMngr;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>(); 
        dropsMngr = GameObject.Find("Managers").GetComponent<DropsSpawnManager>();
        if (playerTarget == null) playerTarget = GameObject.FindGameObjectWithTag("Player");       
    }

    void Update()
    {
        if (!isDead)
        {
            if (playerTarget == null) return;

            else agent.SetDestination(playerTarget.transform.position);

            if (attackCooldown > 0)
            {
                attackCooldown -= Time.deltaTime;
            }

        }
        else
        {
            despawnCooldown += Time.deltaTime;
            if (despawnCooldown >= 5)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            zombiesound.PlayOneShot(attackSound);

            animator.SetBool("IsAttacking", true);
            Player.instance.health -= 0.1f;
        }
        else if (other.tag == "Bullet")
        {
            Player.instance.killCount++;
            animator.SetBool("IsDead", true);

            zombiesound.PlayOneShot(deathSounds[UnityEngine.Random.Range(0,3)]);
            if (!isDead)
            {
                Destroy(other.gameObject); //destroy bullet
                Destroy(ZombieMapIndicator);
                Destroy(GetComponent<BoxCollider>());
                Destroy(GetComponent<NavMeshAgent>());
            }
            isDead = true;

            //Drop
            rndSpawnChance = UnityEngine.Random.Range(0, 100);
            if (rndSpawnChance > 90)
            {
                rndSpawnChance = UnityEngine.Random.Range(0, 2);
                dropsMngr.SpawnUsableObject(rndSpawnChance, transform.position, transform.rotation);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && attackCooldown <= 0f)
        {
            zombiesound.PlayOneShot(attackSound);

            animator.SetBool("IsAttacking", true);
            Player.instance.health -= 0.1f;
            attackCooldown = biteDelay; 
        }
    }
}
