using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour
{
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private AudioClip[] deathSounds;

    [SerializeField] private AudioSource zombiesound;

    private Animator animator;

    private GameObject playerTarget;
    private NavMeshAgent agent;

    private float biteDelay = 0.5f;
    private float attackCooldown = 0f;

    private bool isDead = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        if (playerTarget == null) playerTarget = GameObject.FindGameObjectWithTag("Player");       
    }

    void Update()
    {
        if (playerTarget == null ) return;

        if (isDead) agent.SetDestination(transform.position);
        else agent.SetDestination(playerTarget.transform.position);

        if (attackCooldown > 0)
        {
            attackCooldown -= Time.deltaTime;
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
            isDead = true;
            animator.SetBool("IsDead", true);

            Destroy(GetComponent<BoxCollider>());

            zombiesound.PlayOneShot(deathSounds[UnityEngine.Random.Range(0,3)]);

            Destroy(other.gameObject); //destroy bullet   
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
