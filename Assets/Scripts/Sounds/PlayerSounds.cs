using System;
using UnityEngine;

public class PlayerSounds : PlayerSystem
{
    public event Action FootstepEvent;
    private PlayerMovement _playerMovement;
    private int _currentFoot = 0;

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

    protected override void Awake()
    {
        base.Awake();
        _playerMovement = GetComponent<PlayerMovement>();
        _timeToNextFootstep = 0;
    }

    void playFootsep() => SFXManager.PlaySoundAtIndex(SFXManager.SoundType.FOOTSTEPS,_currentFoot++%2);

    void Update()
    {
        if(_playerMovement.IsMoving()&& _timeToNextFootstep <=0)
        {
            FootstepEvent.Invoke();
            _timeToNextFootstep = 1 / (_footstep_frequency * (_player.CurrentSpeed/3f));
        }

        _timeToNextFootstep-=Time.deltaTime;
    }
}
