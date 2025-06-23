using System;
using UnityEngine;

/// <summary>
/// A singleton manager that houses sound effects and plays them.
/// </summary>
[RequireComponent(typeof(AudioSource)), ExecuteInEditMode]
public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance;

    // generice sound effects, with variants
    private AudioSource _audioSource;
    [SerializeField] private SoundList [] _soundList;

    void Awake()
    {
        //singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        _audioSource = GetComponent<AudioSource>();  
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

    /// <summary>
    /// Plays a particular variant of a sound clip at a given index. Highly specific usage, and <c>PlaySound</c> can probably suit your needs, unless you know what you are doing.
    /// </summary>
    /// <param name="sound">The type of sound clip</param>
    /// <param name="index">The index of the variant to play</param>
    public static void PlaySoundAtIndex(SoundType sound, int index = 0, float volume = 1f)
    {
        AudioClip[] clips = Instance?._soundList[(int)sound].Sounds;
        Instance?._audioSource.PlayOneShot(clips[index], volume);
    }

    /// <summary>
    /// For playing generic sounds that have multiple variants
    /// </summary>
    /// <param name="sound">The type of sound to play</param>
    public static void PlaySound(SoundType sound, float volume = 1f)
    {
        AudioClip[] clips = Instance?._soundList[(int)sound].Sounds;
        Instance?._audioSource.PlayOneShot(clips[UnityEngine.Random.Range(0, clips.Length)], volume);
    }

    /// <summary>
    /// Plays a given <c>AudioClip</c> object. Used for playing unique sounds that are attached to specific objects
    /// </summary>
    public static void PlayClip(AudioClip audioClip,float volume=1f)
    {
        Instance?._audioSource.PlayOneShot(audioClip,volume);
    }

    /// <summary>
    /// For playing generic sounds that have multiple variants, at a given position in space.
    /// </summary>
    /// <param name="sound">The type of sound to play</param>
    /// <param name="position">The world position to play at</param>
    public static void PlaySoundAtPosition(SoundType sound,Vector3 position,float volume=1f)
    {
        AudioClip [] clips = Instance?._soundList[(int) sound].Sounds;
        AudioSource.PlayClipAtPoint(clips[UnityEngine.Random.Range(0,clips.Length)],position,volume);
    }

    /// <summary>
    /// Convert a <c>SoundType</c> <c>enum</c> into one of its <c>AudioClips</c> at random.
    /// </summary>
    public static AudioClip GetAudioClip(SoundType sound)
    {
        AudioClip[] clips = Instance?._soundList[(int)sound].Sounds;
        return clips[UnityEngine.Random.Range(0, clips.Length)];
    }

    /// <summary>
    /// Gets a specific index <c>AudioClip</c> for a given <c>SoundType</c>
    /// </summary>
    public static AudioClip GetAudioClipAt(SoundType sound, int index)
    {
        AudioClip[] clips = Instance?._soundList[(int)sound].Sounds;
        return clips[index];
    }

    /// <summary>
    /// Instruct an <c>AudioSource</c> to loop its sound clip.
    /// </summary>
    /// <param name="audioSource">The source to loop</param>
    public static void LoopClip(AudioSource audioSource, float volume = 1f)
    {
        if (!audioSource.loop) Debug.LogWarning($"{audioSource} is not a looping AudioSource. Setting to active loop...");
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
        CREEPY_NOISE,
        HAMMER_BONK,
        SUIT_EQUIP,
        INTERACT_FAIL
    }
    public enum ALARM_INTENSITY
    {
        LOW,
        MID,
        HIGH
    }

    [Serializable]
    private struct SoundList
    {
        public AudioClip[] Sounds {get => sounds;}
        [HideInInspector] public string name;
        [SerializeField] private AudioClip [] sounds;
    }
}
