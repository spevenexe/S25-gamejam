using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

/// <summary>
/// Script that handles moving the player (duh).
/// </summary>
public class PlayerMovement : PlayerSystem
{
    private Rigidbody _rb;
    [SerializeField] private float _speed;
    private Coroutine _footsteps;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _player.CurrentSpeed = _speed;
        _player.BaseSpeed = _speed;
    }

    void FixedUpdate()
    {
        // matrix rotation around camera angle
        float angle = -_player.PlayerOrientation.eulerAngles.y * Mathf.PI / 180f;
        float rotatedXDirection = _player.MoveDirection.x * Mathf.Cos(angle) - _player.MoveDirection.y * Mathf.Sin(angle);
        float rotatedYDirection = _player.MoveDirection.x * Mathf.Sin(angle) + _player.MoveDirection.y * Mathf.Cos(angle);
        _player.MoveDirection = new Vector2(rotatedXDirection, rotatedYDirection);

        _rb.linearVelocity = new Vector3(_player.MoveDirection.x * _player.CurrentSpeed, _rb.linearVelocity.y, _player.MoveDirection.y * _player.CurrentSpeed);
    }

    /// <summary>
    /// Returns whether the player is moving.
    /// </summary>
    public bool IsMoving()
    {
        float v_x = _rb.linearVelocity.x;
        float v_z = _rb.linearVelocity.z;
        return v_x * v_x + v_z * v_z > 0.1 * 0.1;
    }
}
