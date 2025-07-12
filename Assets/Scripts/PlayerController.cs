using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float health = 100f;
    public float maxHealth = 100f;
    public float speed = 5f;
    public bool isDead = false;
    private float horizontalInput;
    private float verticalInput;
    public GameObject cameraObject;

    public GameObject projectilePrefab;
    public float projectileSpwanOffset = 1.5f;

    public ParticleSystem deathParticles;
    public float deathParticleScale = 0.1f;

    public delegate void OnHealthChanged(float currentHealth);
    public static event OnHealthChanged onHealthChanged;
    public static event Action onPlayerDied;

    void Start()
    {
        health = maxHealth;
        UpdateHealthUI();
    }

    void Update()
    {
        if (isDead) return; // Prevent further updates if player is dead
        GetInput();
        MovePlayer();
        ShootProjectile();
        LimitHealth();

        if (Input.GetKey(KeyCode.I))
        {
            Die();
        }

    }

    void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
    }

    void MovePlayer()
    {
        Vector3 cameraForward = cameraObject.transform.forward;
        Vector3 cameraRight = cameraObject.transform.right;

        cameraForward.y = 0;
        cameraRight.y = 0;

        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 moveDirection = cameraRight * horizontalInput + cameraForward * verticalInput;
        transform.position += moveDirection * speed * Time.deltaTime;
    }

    void LimitHealth()
    {
        if (health > maxHealth)
        {
            health = maxHealth;
        }
        else if (health < 0)
        {
            health = 0;
        }
    }
    void ShootProjectile()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse button click
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 2f);

            if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            {
                Vector3 direction = (hit.point - transform.position).normalized;
                Quaternion rotation = Quaternion.LookRotation(direction); // face the hit point
                Instantiate(projectilePrefab, transform.position + direction * projectileSpwanOffset, rotation); // spawn with correct forward
            }
        }
    }

    public void UpdateHealthUI()
    {
        onHealthChanged?.Invoke(health);
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
        UpdateHealthUI();
    }

    public void Heal(float amount)
    {
        health += amount;
        LimitHealth();
        UpdateHealthUI();
        Debug.Log("Player healed. Current health: " + health);
    }

    public void Die()
    {
        SpawnDeathParticles();
        isDead = true;
        onPlayerDied?.Invoke();
        Debug.Log("Player has died.");
        TurnOffComponents(); // Disable components that should not be active when dead
    }

    public void ResetPlayer()
    {
        transform.position = Vector3.zero; // Reset position to origin or a specific spawn point
        transform.rotation = Quaternion.identity; // Reset rotation to default
        health = maxHealth; // Reset health
        isDead = false; // Set isDead to false
        TurnOnComponents(); // Enable components that should be active when alive
        UpdateHealthUI(); // Update health UI
        Debug.Log("Player has been reset.");
    }
    public void Resurrect(float spawnedHealth)
    {
        health = spawnedHealth; // Set health
        isDead = false; // Set isDead to false
        TurnOnComponents(); // Enable components that should be active when alive
        UpdateHealthUI(); // Update health UI
        Debug.Log("Player has resurrected.");
    }
    public void TurnOffComponents()
    {
        // Disable all components that should not be active when the player is dead
        GetComponent<Rigidbody>().isKinematic = true; // Prevent physics interactions
        GetComponent<PlayerController>().enabled = false; // Disable player controls
        cameraObject.GetComponent<CameraController>().enabled = false; // Disable camera controls
        GetComponent<MeshRenderer>().enabled = false; // Hide player mesh
        GetComponent<Collider>().enabled = false; // Disable collision detection
        // Add any other components that should be disabled here
    }

    public void TurnOnComponents()
    {
        // Enable all components that should be active when the player is alive
        GetComponent<Rigidbody>().isKinematic = false; // Enable physics interactions
        GetComponent<PlayerController>().enabled = true; // Enable player controls
        cameraObject.GetComponent<CameraController>().enabled = true; // Enable camera controls
        GetComponent<MeshRenderer>().enabled = true; // Show player mesh
        GetComponent<Collider>().enabled = true; // Enable collision detection
        // Add any other components that should be enabled here
    }
    public void SpawnDeathParticles()
    {
        if (deathParticles != null)
        {
            ParticleSystem particles = Instantiate(deathParticles, transform.position, Quaternion.identity);
            particles.transform.localScale = Vector3.one * deathParticleScale;
            particles.Play();
            Destroy(particles.gameObject, particles.main.duration);
        }
    }
}
