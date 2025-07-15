using UnityEngine;

public class ExplosiveProjectile : ProjectileBase
{
    [Header("Explosion Settings")]
    public float explosionRadius = 5f;
    public float explosionForce = 700f;
    public float explosionUpwardModifier = 0.5f;

    private bool hasExplodedByTimeout = false;

    protected override void OnHit()
    {
        if (hasHit) return;

        hasHit = true;

        // VFX handled by base
        SpawnOnHitEffect();

        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("EnemyHitbox"))
            {
                GameObject enemy = hit.transform.root.gameObject;

                if (enemy.CompareTag("Enemy"))
                {
                    ApplyDamage(enemy);
                    ApplyExplosionForce(enemy);
                }
            }
        }

        Destroy(gameObject);
    }

    protected void OnDestroy()
    {
        // Süre dolduğunda Destroy edildiğinde hala patlamadıysa, patlat:
        if (!hasHit && !hasExplodedByTimeout)
        {
            hasExplodedByTimeout = true;
            OnHit(); // Bu şekilde süre dolduğunda da patlar
        }
    }

    private void ApplyExplosionForce(GameObject enemy)
    {
        Rigidbody enemyRb = enemy.GetComponent<Rigidbody>();
        if (enemyRb != null)
        {
            enemyRb.AddExplosionForce(explosionForce, transform.position, explosionRadius, explosionUpwardModifier, ForceMode.Impulse);
        }
    }
}
