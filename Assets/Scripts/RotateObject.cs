using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [Header("Rotation Speed (degrees per second)")]
    public float rotationSpeedX = 45f;
    public float rotationSpeedY = 30f;
    public float rotationSpeedZ = 15f;

    void Update()
    {
        RotateSmoothly();
    }

    private void RotateSmoothly()
    {
        Vector3 rotationThisFrame = new Vector3(rotationSpeedX, rotationSpeedY, rotationSpeedZ) * Time.deltaTime;
        transform.Rotate(rotationThisFrame, Space.Self);
    }
}
