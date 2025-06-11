using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerMovement : PlayerSystem
{
    private Rigidbody _rb;
    [SerializeField] private float _speed;
    [SerializeField] private Transform playerOrientation;
    private Coroutine _footsteps;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        player.CurrentSpeed = _speed;
        player.BaseSpeed = _speed;
    }

    void FixedUpdate()
    {
        // matrix rotation around camera angle
        float angle = - playerOrientation.eulerAngles.y * Mathf.PI / 180f;
        float rotatedXDirection = player.MoveDirection.x * Mathf.Cos(angle) - player.MoveDirection.y * Mathf.Sin(angle);
        float rotatedYDirection = player.MoveDirection.x * Mathf.Sin(angle) + player.MoveDirection.y * Mathf.Cos(angle);
        player.MoveDirection = new Vector2(rotatedXDirection,rotatedYDirection);

        _rb.linearVelocity = new Vector3(player.MoveDirection.x * player.CurrentSpeed,_rb.linearVelocity.y,player.MoveDirection.y * player.CurrentSpeed);
    }    

    public bool IsMoving()
    {
        float v_x = _rb.linearVelocity.x;
        float v_z = _rb.linearVelocity.z;
        return v_x*v_x + v_z*v_z > 0.1*0.1;
    }
}
