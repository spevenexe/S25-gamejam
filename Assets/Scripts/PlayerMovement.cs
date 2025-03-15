using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 _moveDir;
    public Vector2 MoveDirection {
        get {return _moveDir;} 
        set {_moveDir = value;}}
    private Rigidbody _rb;
    [SerializeField] private float _speed;
    [SerializeField] private Transform playerOrientation;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // matrix rotation around camera angle
        float angle = - playerOrientation.eulerAngles.y * Mathf.PI / 180f;
        float rotatedXDirection = _moveDir.x * Mathf.Cos(angle) -  _moveDir.y * Mathf.Sin(angle);
        float rotatedYDirection = _moveDir.x * Mathf.Sin(angle) + _moveDir.y * Mathf.Cos(angle);
        // Debug.Log("Before, "+_moveDir);
        _moveDir = new Vector2(rotatedXDirection,rotatedYDirection);
        // Debug.Log("After, "+_moveDir);

        _rb.linearVelocity = new Vector3(_moveDir.x * _speed,_rb.linearVelocity.y,_moveDir.y * _speed);
    }    
}
