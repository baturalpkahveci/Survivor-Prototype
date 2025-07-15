using System.Collections;
using UnityEngine;

public class UpgradePickup : MonoBehaviour
{
    [Tooltip("This must be a ScriptableObject that implements IUpgradeEffect")]
    public ScriptableObject upgradeEffectSO;
    public GameObject objectToHide;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (upgradeEffectSO is IUpgradeEffect effect)
        {
            if (effect.IsTimed)
            {
                StartCoroutine(HandleTimedEffect(other.gameObject, effect));
            }
            else
            {
                effect.Apply(other.gameObject);
                Destroy(gameObject); // Destroy if one shot
            }

            if (objectToHide) objectToHide.SetActive(false);
        }
        else
        {
            Debug.LogError("Assigned ScriptableObject does not implement IUpgradeEffect!");
        }
    }

    private IEnumerator HandleTimedEffect(GameObject player, IUpgradeEffect effect)
    {
        effect.Apply(player);
        Debug.Log(effect + " is applied.");
        yield return new WaitForSeconds(effect.Duration);
        effect.Remove(player);
        Debug.Log(effect + " is removed.");
        Destroy(gameObject); 
    }
}
