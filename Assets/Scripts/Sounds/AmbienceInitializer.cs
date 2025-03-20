using System.Collections.Generic;
using UnityEngine;

class AmbienceInitializer : MonoBehaviour
    {
        // ambient sound effects. These are only used at the start and play forever. If you want finer control of the sound, use LoopClip() from a difference script.
        [SerializeField] private AudioClip [] _initAmbiencesClips;
        private List<AudioSource> _initAmbienceSources = new();
        [SerializeField] private AudioSource sourcePrefab;
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
        }
    }
