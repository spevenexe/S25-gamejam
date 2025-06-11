using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private float _lookSensitivity;
    private float _xRotation=0,_yRotation=0;
    [SerializeField] private Transform _playerOrientation;
    [SerializeField] private PData _player;

    private ViewBobbing _viewBobber;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _viewBobber = GetComponent<ViewBobbing>();
        _viewBobber.PlayerOrientation = _playerOrientation;

        if (_player != null)
            return;
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if(playerObject!=null) _player = playerObject.GetComponent<PData>();
        else Debug.LogWarning($"No player found. _player set to default ({_player})");
    }

    void Update()
    {
        _xRotation -=_player.LookDirection.y*_lookSensitivity;
        _xRotation = Mathf.Clamp(_xRotation,-90f,90f);
        _yRotation +=_player.LookDirection.x*_lookSensitivity;

        transform.rotation = Quaternion.Euler(_xRotation,_yRotation,0f);
        _playerOrientation.rotation = Quaternion.Euler(0f,_yRotation,0f);

        _viewBobber.Bob(_player.CurrentSpeed/_player.BaseSpeed);
    }
}
