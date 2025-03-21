using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(AudioSource)),ExecuteInEditMode]
public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance;

    // generice sound effects, with variants
    private AudioSource _audioSource;
    [SerializeField] private SoundList [] _soundList;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

#if UNITY_EDITOR
    void OnEnable()
    {
        string [] names = Enum.GetNames(typeof(SoundType));
        Array.Resize(ref _soundList,names.Length);
        for(int i = 0; i < _soundList.Length; i++)
            _soundList[i].name = names[i];
    }
#endif

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();  
    }
    // you probably don't need this, but this adds greater flexibility
    public static void PlaySoundAtIndex(SoundType sound,int index=0, float volume=1f)
    {
        AudioClip [] clips = Instance._soundList[(int) sound].Sounds;
        Instance._audioSource.PlayOneShot(clips[index],volume);
    }

    // for playing generic sounds that have multiple variants (e.g. footsteps)
    public static void PlaySound(SoundType sound,float volume=1f)
    {
        AudioClip [] clips = Instance._soundList[(int) sound].Sounds;
        Instance._audioSource.PlayOneShot(clips[UnityEngine.Random.Range(0,clips.Length)],volume);
    }

    // for playing unique sound types that are attached to specific objects
    public static void PlayClip(AudioClip audioClip,float volume=1f)
    {
        Instance._audioSource.PlayOneShot(audioClip,volume);
    }

    // for playing generic sounds that have multiple variants (e.g. footsteps)
    public static void PlaySoundAtPosition(SoundType sound,Vector3 position,float volume=1f)
    {
        AudioClip [] clips = Instance._soundList[(int) sound].Sounds;
        AudioSource.PlayClipAtPoint(clips[UnityEngine.Random.Range(0,clips.Length)],position,volume);
    }

    public static AudioClip GetAudioClip(SoundType sound)
    {
        AudioClip [] clips = Instance._soundList[(int) sound].Sounds;
        return clips[UnityEngine.Random.Range(0,clips.Length)];
    }

    public static AudioClip GetAudioClipAt(SoundType sound,int index)
    {
        AudioClip [] clips = Instance._soundList[(int) sound].Sounds;
        return clips[index];
    }

    public static void LoopClip(AudioSource audioSource,float volume=1f)
    {
        if (!audioSource.loop)Debug.LogWarning($"{audioSource} is not a looping audio. Setting to active loop...");
        audioSource.loop = true;
        audioSource.volume = volume;
        if (!audioSource.isPlaying) audioSource.Play();
    }
    
    public enum SoundType
    {
        FOOTSTEPS,
        PICKUP,
        BUTTON,
        CRASH,
        ENGINE_BREAK,
        LEVER,
        ALARM,
        ITEM_CLANG,
        CREEPY_NOISE
    }
    public enum ALARM_INTENSITY
    {
        LOW,
        MID,
        HIGH
    }

    [System.Serializable]
    private struct SoundList
    {
        public AudioClip[] Sounds {get => sounds;}
        [HideInInspector] public string name;
        [SerializeField] private AudioClip [] sounds;
    }
}
