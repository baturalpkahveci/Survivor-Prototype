using UnityEngine;

[CreateAssetMenu(fileName = "SpeedBoostEffect", menuName = "UpgradeEffects/SpeedBoostEffect")]
public class SpeedBoostEffect : ScriptableObject, IUpgradeEffect
{
    public float speedMultiplier = 2f;
    public float duration = 5f;

    public bool IsTimed => true;

    public float Duration => duration;

    public void Apply(GameObject player)
    {
        player.GetComponent<PlayerController>().speed *= speedMultiplier;
    }

    public void Remove(GameObject player)
    {
        player.GetComponent<PlayerController>().speed /= speedMultiplier;
    }
}
