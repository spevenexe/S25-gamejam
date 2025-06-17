using System;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Script that bobs the camera while moving.
/// </summary>
public class ViewBobbing : MonoBehaviour
{
    [SerializeField] private Rigidbody _playerRB;
    [SerializeField] private float _bobTopDistance = 0.2f;
    [SerializeField] private float _bobBottomDistance = 0.2f;
    [SerializeField] private float _upBobStrength = 0.005f;
    [SerializeField] private float _downBobStrength = 0.01f;
    private float _bobStrength;
    private float _bobTarget_Y;
    private enum lerpDir{
        up,
        down
    }
    private lerpDir _lerpDirection;

    [SerializeField] private PData _player;
    void Start()
    {
        // set up the first lerp
        _bobTarget_Y = _player.PlayerOrientation.position.y + _bobTopDistance;
        _bobStrength = _upBobStrength;

        // find the player, if no data is specified
        if (_player == null) _player = Utils.getPlayer();
    }

    void Update()
    {
        Bob(_player.CurrentSpeed/_player.BaseSpeed);
    }

    /// <summary>
    /// Bobs the camera for the current frame. Accomplishes this by lerping the camera towards one of three target points:
    /// <list type="number">
    /// <item>
    /// <description> A fixed point above the camera, determined by 
    /// <c>_bobTopDistance</c>. </description>
    /// </item>
    /// 
    /// <item>
    /// <description> A fixed point below the camera, determined by 
    /// <c>_bobBottomDistance</c>. </description>
    /// </item>
    /// 
    /// <item>
    /// <description> The camera's resting height. </description>
    /// </item>
    /// </list> 
    /// </summary>
    /// <param name="speed">The player's speed. Determines how fast the lerp moves towards the target point.</param>
    private void Bob(float speed)
    {
        float v_x = _playerRB.linearVelocity.x;
        float v_z = _playerRB.linearVelocity.z;
        // if speed is non-zero...
        if (v_x * v_x + v_z * v_z > 0.1 * 0.1)
        {
            // and the camera is close to the target...
            if (Mathf.Abs(transform.position.y - _bobTarget_Y) < 0.1f)
            {
                // switch directions
                _lerpDirection = (lerpDir)((int)(_lerpDirection + 1) % 2);
                _bobTarget_Y = _player.PlayerOrientation.position.y + ((_lerpDirection == lerpDir.up) ? _bobTopDistance : -_bobBottomDistance);
                _bobStrength = (_lerpDirection == lerpDir.up) ? _upBobStrength : _downBobStrength;
            }

        }
        else
        {
            // the player isn't moving, reset the target to the resting point
            _bobTarget_Y = _player.PlayerOrientation.position.y;
            _bobStrength = _upBobStrength;
            _lerpDirection = lerpDir.up;
        }

        float adjustedBobStrength = _bobStrength * speed;
        float newY = Mathf.Lerp(transform.position.y, _bobTarget_Y, adjustedBobStrength);
        transform.position = new Vector3(_player.PlayerOrientation.position.x, newY, _player.PlayerOrientation.position.z);
    }
}
