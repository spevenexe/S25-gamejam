using UnityEngine;

/// <summary>
/// Script that handles moving the player (duh).
/// </summary>
public class PlayerMovement : PlayerSystem
{
    private Rigidbody _rb;
    [SerializeField] private float _speed;

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
        return Utils.IsMoving(_rb);
    }
}
