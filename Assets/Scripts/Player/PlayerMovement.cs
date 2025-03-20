using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 _moveDir;
    public Vector2 MoveDirection {
        get {return _moveDir;} 
        set {_moveDir = value;}}
    private Rigidbody _rb;
    [SerializeField] private float _speed;
    public float BaseSpeed {get; private set;}
    [SerializeField] private Transform playerOrientation;
    private Coroutine _footsteps;
    [SerializeField] public float CurrentSpeed {get; private set;}
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        CurrentSpeed = _speed;
    }

    void FixedUpdate()
    {
        // matrix rotation around camera angle
        float angle = - playerOrientation.eulerAngles.y * Mathf.PI / 180f;
        float rotatedXDirection = _moveDir.x * Mathf.Cos(angle) - _moveDir.y * Mathf.Sin(angle);
        float rotatedYDirection = _moveDir.x * Mathf.Sin(angle) + _moveDir.y * Mathf.Cos(angle);
        // Debug.Log("Before, "+_moveDir);
        _moveDir = new Vector2(rotatedXDirection,rotatedYDirection);
        // Debug.Log("After, "+_moveDir);

        _rb.linearVelocity = new Vector3(_moveDir.x * CurrentSpeed,_rb.linearVelocity.y,_moveDir.y * CurrentSpeed);
    }    

    public bool IsMoving()
    {
        float v_x = _rb.linearVelocity.x;
        float v_z = _rb.linearVelocity.z;
        return v_x*v_x + v_z*v_z > 0.1*0.1;
    }
}
