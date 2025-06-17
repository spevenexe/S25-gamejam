using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    private float _xRotation=0,_yRotation=0;
    [SerializeField] private PData _player;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        // find the player, if none provided
        if (_player == null) _player = Utils.getPlayer();
    }

    void Update()
    {
        // Turn the camera. Keep in mind how axes are named
        _xRotation -=_player.LookDirection.y*_player.LookSensitivity;
        _xRotation = Mathf.Clamp(_xRotation,-90f,90f);
        _yRotation +=_player.LookDirection.x*_player.LookSensitivity;

        transform.rotation = Quaternion.Euler(_xRotation,_yRotation,0f);
        // the orientation only tracks the y rotation. so the player can't look up and down, and change 
        _player.PlayerOrientation.rotation = Quaternion.Euler(0f,_yRotation,0f);
    }
}
