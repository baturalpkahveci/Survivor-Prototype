using UnityEngine;

public class FocalPoint : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        Quaternion rot = transform.rotation;
        rot.x = 0f;
        rot.z = 0f;
        transform.rotation = rot;
    }
}
