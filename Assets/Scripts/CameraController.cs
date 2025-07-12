using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject focalPoint; // The point the camera will look at
    public float rotationSpeed = 90f; // Degrees per second
    public KeyCode turnRightKey = KeyCode.E;
    public KeyCode turnLeftKey = KeyCode.Q;

    // Update is called once per frame
    void Update()
    {
        transform.rotation.SetLookRotation(focalPoint.transform.position - transform.position, Vector3.up);

        if (turnRightKey != KeyCode.None && Input.GetKey(turnRightKey))
        {
            focalPoint.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }
        if (turnLeftKey != KeyCode.None && Input.GetKey(turnLeftKey))
        {
            focalPoint.transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime);
        }

    }

}
