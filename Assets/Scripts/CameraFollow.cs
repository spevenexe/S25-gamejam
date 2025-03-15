using UnityEngine;

public class CameraFollow : MonoBehaviour {
    public Transform PlayerCameraPosition;

    void Update()
    {
        transform.position = PlayerCameraPosition.position;
    }
}