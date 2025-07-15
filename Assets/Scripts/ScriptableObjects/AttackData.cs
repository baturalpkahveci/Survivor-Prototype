using UnityEngine;

[CreateAssetMenu(fileName = "AttackData", menuName = "Combat/AttackData")]
public class AttackData : ScriptableObject
{
    public string attackName;
    public GameObject projectilePrefab;
    public float damage = 10f;
    public float cooldown = 1f;

    public virtual bool CanUse()
    {
        // Override edilebilir: Örn. enerji, mana gibi sistem varsa
        return true;
    }
}