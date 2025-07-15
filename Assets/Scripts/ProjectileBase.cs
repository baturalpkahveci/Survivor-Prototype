using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ProjectileBase : MonoBehaviour
{
    private float damage = 35f;
    public float speed = 40f;
    public float lifeTime = 5f;

    [Header("Optional Hit Effect")]
    public GameObject onHitEffectPrefab;
    public float onHitEffectMinScale = 0.8f;
    public float onHitEffectMaxScale = 1.3f;

    protected Rigidbody rb;
    protected bool hasHit = false;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        StartCoroutine(DestroyAfterTime(lifeTime));
        InitMovement();
    }

    protected virtual void InitMovement()
    {
        rb.linearVelocity = transform.forward * speed;
    }

    public void SetDamage(float dmg)
    {
        damage = dmg;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (hasHit) return;

        if (other.CompareTag("EnemyHitbox"))
        {
            GameObject enemy = other.transform.root.gameObject;
            if (enemy.CompareTag("Enemy"))
            {
                ApplyDamage(enemy);
                SpawnOnHitEffect();
                OnHit();
            }
        }
    }

    protected virtual void ApplyDamage(GameObject target)
    {
        var enemyScript = target.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            enemyScript.TakeDamage(damage);
        }
    }

    protected virtual void OnHit()
    {
        hasHit = true;
        Destroy(gameObject);
    }

    protected IEnumerator DestroyAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }

    protected void SpawnOnHitEffect()
    {
        if (onHitEffectPrefab != null)
        {
            GameObject fx = Instantiate(onHitEffectPrefab, transform.position, Quaternion.identity);

            float randomScale = Random.Range(onHitEffectMinScale, onHitEffectMaxScale);
            fx.transform.localScale = Vector3.one * randomScale;

            ParticleSystem ps = fx.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                ps.Play();
                Destroy(fx, ps.main.duration + ps.main.startLifetime.constantMax);
            }
            else
            {
                Destroy(fx, 2f); // fallback
            }
        }
    }
}