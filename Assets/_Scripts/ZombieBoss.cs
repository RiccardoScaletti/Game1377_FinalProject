using System;
using UnityEngine;
using UnityEngine.AI;

public class ZombieBoss : MonoBehaviour
{
    public static ZombieBoss instance { get; private set; }

    [SerializeField] private AudioSource attack;
    [SerializeField] private GameObject FireBallPrefab;
    private GameObject playerTarget;
    private NavMeshAgent agent;

    private float biteDelay = 0.5f;
    private float attackCooldown = 0.25f;
    private float fireballCooldown = 2f;
    private int healthPoints = 70;

    public Action onBossDefeated;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (playerTarget == null) playerTarget = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (playerTarget == null) return;
        agent.SetDestination(playerTarget.transform.position);

        if (attackCooldown > 0)
        {
            attackCooldown -= Time.deltaTime;
        }
       
        fireballCooldown -= Time.deltaTime;

        if (fireballCooldown <= 0)
        {
            FireBall();
        }
    }

    private void FireBall()
    {
        Debug.Log("Fireball");
        Instantiate(FireBallPrefab, transform.position, transform.rotation);
        fireballCooldown = 2;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            attack.Play();
            Player.instance.health -= 0.25f;
            attackCooldown = biteDelay;
        }
        else if (other.tag == "Bullet")
        {
            healthPoints -= Player.instance.currentWeapon.dmg;
            if (healthPoints <= 0)
            {
                Destroy(gameObject);
                onBossDefeated?.Invoke();
            }
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
    }
}
