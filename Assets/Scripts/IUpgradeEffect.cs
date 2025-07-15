using UnityEngine;

public interface IUpgradeEffect
{
    void Apply(GameObject player);
    void Remove(GameObject player);
    bool IsTimed { get; }
    float Duration { get; }
}
