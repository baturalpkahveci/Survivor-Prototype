using UnityEngine;

public class AttackComponent : MonoBehaviour
{
    public AttackData[] attacks;
    private float[] lastUsedTimes;
    public float projectileSpwanOffset = 1.5f;

    private void Start()
    {
        lastUsedTimes = new float[attacks.Length];
    }

    public void TryAttack(int index)
    {
        if (index < 0 || index >= attacks.Length) return;

        AttackData attack = attacks[index];

        if (CanUseAttack(index, attack))
        {
            ExecuteAttack(attack);
            lastUsedTimes[index] = Time.time;
        }
        else
        {
            Debug.Log($"Attack {attack.attackName} is not ready or conditions are unmet.");
        }
    }

    private bool CanUseAttack(int index, AttackData attack)
    {
        bool cooldownReady = Time.time - lastUsedTimes[index] >= attack.cooldown;
        bool conditionMet = attack.CanUse();
        return cooldownReady && conditionMet;
    }

    private void ExecuteAttack(AttackData attack)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 2f);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            Vector3 direction = (hit.point - transform.position).normalized;
            Quaternion rotation = Quaternion.LookRotation(direction); // face the hit point
            GameObject spawned = Instantiate(attack.projectilePrefab, transform.position + direction * projectileSpwanOffset, rotation); // spawn with correct forward
            ProjectileBase projectileScript = spawned.GetComponent<ProjectileBase>();
            projectileScript.SetDamage(attack.damage);
        }
    }
}
