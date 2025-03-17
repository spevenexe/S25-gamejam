using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private float _lookSensitivity;
    private Vector2 _lookDir;
    public Vector2 LookDirection {
        get {return _lookDir;} 
        set {_lookDir = value;}}
    private float _xRotation=0,_yRotation=0;
    [SerializeField] private Transform _playerOrientation;

    private ViewBobbing _viewBobber;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _viewBobber = GetComponent<ViewBobbing>();
        _viewBobber.PlayerOrientation = _playerOrientation;
    }

    void Update()
    {
        _xRotation -=_lookDir.y*_lookSensitivity;
        _xRotation = Mathf.Clamp(_xRotation,-90f,90f);
        _yRotation +=_lookDir.x*_lookSensitivity;

        transform.rotation = Quaternion.Euler(_xRotation,_yRotation,0f);
        _playerOrientation.rotation = Quaternion.Euler(0f,_yRotation,0f);

        _viewBobber.Bob();
    }
}
