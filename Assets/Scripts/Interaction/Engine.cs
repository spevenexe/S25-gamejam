using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Engine : MonoBehaviour
{
    private AudioSource _audioSource;
    
    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void Break()
    {
        _audioSource.Stop();
        SFXManager.PlaySound(SFXManager.SoundType.ENGINE_BREAK);
    }

    public void StartEngine() 
    {
        if (!_audioSource.isPlaying) SFXManager.LoopClip(_audioSource);
    }
}
