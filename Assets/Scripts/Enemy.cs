using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public float health = 100f;
    public float speed = 10f;
    public float damage = 5f;
    public int score = 10;
    public float wanderIntervalMin = 1f; // Minimum time between wanders
    public float wanderIntervalMax = 4f; // Maximum time between wanders
    private Rigidbody rb;
    private GameObject player;
    private PlayerController playerController;
    private Vector3 moveDirection;
    public ParticleSystem deathParticles;
    public float deathParticleScaleMin = 0.1f;
    public float deathParticleScaleMax = 0.6f;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerController = player.GetComponent<PlayerController>();
        }
    }

    void FixedUpdate()
    {
        if (player == null || playerController.isDead)
        {
            Wander();
        }
        else
        {
            MoveTowardsPlayer();
        }
        
    }

    void MoveTowardsPlayer()
    {
        if (player == null) return;

        moveDirection = player.transform.position - transform.position;
        moveDirection.y = 0;
        moveDirection.Normalize();

        rb.MovePosition(rb.position + moveDirection * speed * Time.fixedDeltaTime);
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
    }

    public IEnumerator Wander()
    {
        while (player == null || playerController.isDead)
        {
            float wanderTime = Random.Range(wanderIntervalMin, wanderIntervalMax);
            yield return new WaitForSeconds(wanderTime);

            Vector3 randomDirection = Random.insideUnitSphere * 5f; // Random direction within a sphere of radius 5
            randomDirection.y = 0; // Keep movement on the horizontal plane
            moveDirection = randomDirection.normalized;

            rb.MovePosition(rb.position + moveDirection * speed * Time.fixedDeltaTime);
        }
    }
    public void Die()
    {
        GameManager.Instance.AddScore(score);
        SpawnDeathParticles();
        Destroy(gameObject);
    }

    public void SpawnDeathParticles()
    {
        if (deathParticles != null)
        {
            ParticleSystem particles = Instantiate(deathParticles, transform.position, Quaternion.Euler(0, Random.Range(0, 360), 0));
            particles.transform.localScale = Vector3.one * Random.Range(deathParticleScaleMin, deathParticleScaleMax);
            particles.Play();
            Destroy(particles.gameObject, particles.main.duration);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            if (playerController != null)
            {
                if (playerController.isDead)
                {
                    return; // Prevent damage if player is dead
                }
                playerController.TakeDamage(damage);
            }
        }
    }
}
