using UnityEngine;

[CreateAssetMenu(fileName = "HealthRestoreEffect", menuName = "UpgradeEffects/HealthRestoreEffect")]
public class HealthRestoreEffect : ScriptableObject, IUpgradeEffect
{
    public int healAmount = 25;

    public bool IsTimed => false;
    public float Duration => 0f;

    public void Apply(GameObject player)
    {
        player.GetComponent<PlayerController>().Heal(healAmount);
    }

    public void Remove(GameObject player) { /* no-op */ }
}
