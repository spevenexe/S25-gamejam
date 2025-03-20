using System;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    public Action FootstepEvent;
    private PlayerMovement _playerMovement;

    [SerializeField] float _footstep_frequency = 2;
    private float _timeToNextFootstep;

    void OnEnable()
    {
        FootstepEvent+=playFootsep;
    }

    void OnDisable()
    {
        FootstepEvent-=playFootsep;
    }

    void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _timeToNextFootstep = 0;
    }

    void playFootsep() => SFXManager.PlaySound(SFXManager.SoundType.FOOTSTEPS);

    void Update()
    {
        if(_playerMovement.IsMoving()&& _timeToNextFootstep <=0)
        {
            FootstepEvent.Invoke();
            _timeToNextFootstep = 1 / (_footstep_frequency * (_playerMovement.CurrentSpeed/3f));
        }

        _timeToNextFootstep-=Time.deltaTime;
    }
}
