using System;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    public Action FootstepEvent;

    void OnEnable()
    {
        FootstepEvent+=playFootsep;
    }

    void OnDisable()
    {
        FootstepEvent-=playFootsep;
    }

    void playFootsep() => SFXManager.PlaySound(SFXManager.SoundType.FOOTSTEPS);
}
