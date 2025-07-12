using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage = 35f;
    public float projectileSpeed = 40f;
    public float projectileLifeTime = 5f;
    public float rotationSpedMin = 80f;
    public float rotationSpedMax = 150f;
    public GameObject projectileObject;
    private Rigidbody rb;
    private bool hasHit = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        StartCoroutine(DestroyAfterTime(projectileLifeTime));
        MoveProjectile();
    }

    IEnumerator DestroyAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }

    void MoveProjectile()
    {
        rb.linearVelocity = transform.forward * projectileSpeed;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyHitbox"))
        {
            if (hasHit) return; // Prevent multiple hits
            hasHit = true; // Mark as hit to prevent further processing
            GameObject enemy = other.transform.root.gameObject;
            if (enemy.CompareTag("Enemy"))
            {
                Enemy enemyScript = enemy.GetComponent<Enemy>();
                enemyScript.TakeDamage(damage);
                Debug.Log("Hit enemy: " + enemy.name + " with damage: " + damage);
                Destroy(gameObject); // Destroy the projectile on hit
            }
        }
    }
}
