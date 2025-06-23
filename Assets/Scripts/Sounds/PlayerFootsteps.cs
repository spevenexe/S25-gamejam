using System;
using UnityEngine;

/// <summary>
/// Manages sounds produced by the player walking.
/// </summary>
public class PlayerFootsteps : PlayerSystem
{
    public event Action FootstepEvent;
    private Rigidbody _rb;
    private int _currentFoot = 0;

    [SerializeField] float _footstep_frequency = 2;
    private float _timeToNextFootstep;

    void OnEnable()
    {
        FootstepEvent += playFootsep;
    }

    void OnDisable()
    {
        FootstepEvent -= playFootsep;
    }

    protected override void Awake()
    {
        base.Awake();
        _rb = GetComponent<Rigidbody>();
        _timeToNextFootstep = 0;
    }

    void playFootsep() => SFXManager.PlaySoundAtIndex(SFXManager.SoundType.FOOTSTEPS, _currentFoot++ % 2);

    void Update()
    {
        if (Utils.IsMoving(_rb) && _timeToNextFootstep <= 0)
        {
            FootstepEvent.Invoke();
            _timeToNextFootstep = 1 / (_footstep_frequency * (_player.CurrentSpeed / 3f));
        }

        _timeToNextFootstep -= Time.deltaTime;
    }
}
