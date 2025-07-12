using UnityEngine;

public class MainMenuCamera : MonoBehaviour
{
    private GameObject player;
    public GameObject cameraFocalPoint;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        SetCamera();
    }

    void SetCamera()
    {
        player.GetComponent<PlayerController>().enabled = false;
        cameraFocalPoint.SetActive(false);
    }
}
