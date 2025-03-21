using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class AmbienceInitializer : MonoBehaviour
{
    // ambient sound effects. These are only used at the start and play forever. If you want finer control of the sound, use LoopClip() from a difference script.
    [SerializeField] private AudioClip [] _initAmbiencesClips;
    private List<AudioSource> _initAmbienceSources = new();
    [SerializeField] private AudioSource sourcePrefab;

    [SerializeField] private int _numThreads = 1;
    [SerializeField] private float _creepyintervalMin = 5f;
    [SerializeField] private float _creepyintervalMax = 5f;
    [SerializeField] private float _volume = 1f;
    void Start()
    {
        foreach(AudioClip clip in _initAmbiencesClips)
        {
            AudioSource source = Instantiate(sourcePrefab);
            _initAmbienceSources.Add(source);
            source.loop = true;
            source.clip = clip;
            SFXManager.LoopClip(source);
        }

        for(int i = 0; i < _numThreads; i++)
            StartCoroutine(PlayCreepyNoise()); 
    }

    private IEnumerator PlayCreepyNoise()
    {
        yield return new WaitForSeconds(_creepyintervalMax);
        while(true)
        {
            float waitTime = UnityEngine.Random.Range(_creepyintervalMin,_creepyintervalMax);
            AudioClip audioClip = SFXManager.GetAudioClip(SFXManager.SoundType.CREEPY_NOISE);
            SFXManager.PlayClip(audioClip,_volume);
            yield return new WaitForSeconds(audioClip.length + waitTime);
        }
    }
}
