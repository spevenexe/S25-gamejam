using System;
using Unity.VisualScripting;
using UnityEngine;

public class ViewBobbing : MonoBehaviour
{
    public Transform PlayerOrientation;
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
    void Start()
    {
        _bobTarget_Y=PlayerOrientation.position.y+_bobTopDistance;
        _bobStrength = _upBobStrength;
    }

    public void Bob(float speed)
    {
        float v_x = _playerRB.linearVelocity.x;
        float v_z = _playerRB.linearVelocity.z;
        if(v_x*v_x + v_z*v_z > 0.1*0.1)
        {
            if(Mathf.Abs(transform.position.y - _bobTarget_Y) < 0.1f)
            {
                _lerpDirection = (lerpDir)((int)(_lerpDirection+1)%2);
                _bobTarget_Y=PlayerOrientation.position.y + ((_lerpDirection == lerpDir.up) ? _bobTopDistance : -_bobBottomDistance); 
                _bobStrength = (_lerpDirection == lerpDir.up) ? _upBobStrength : _downBobStrength;
            }

        }else 
        {
            _bobTarget_Y = PlayerOrientation.position.y;
            _bobStrength = _upBobStrength;
            _lerpDirection = lerpDir.up;
        }
        
        float adjustedBobStrength = _bobStrength * speed;
        float newY =  Mathf.Lerp(transform.position.y,_bobTarget_Y,_bobStrength);
        transform.position = new Vector3(PlayerOrientation.position.x,newY,PlayerOrientation.position.z);
    }
}
