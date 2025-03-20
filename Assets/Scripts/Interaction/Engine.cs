using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Engine : MonoBehaviour
{
    private AudioSource _audioSource;
    [SerializeField] float _breakingVolume;
    
    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        StartEngine();
    }

    public void Break()
    {
        _audioSource.Stop();
        SFXManager.PlaySound(SFXManager.SoundType.ENGINE_BREAK,_breakingVolume*_audioSource.volume);
    }

    public void StartEngine() 
    {
        if (!_audioSource.isPlaying) SFXManager.LoopClip(_audioSource);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Engine))]
public class EngineInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Engine engine = (Engine) target;
        if(GUILayout.Button("Break",GUILayout.Width(90f)))
        {
            engine.Break();
        }
        
        if(GUILayout.Button("Fix",GUILayout.Width(90f)))
        {
            engine.StartEngine();
        }
    }
}
#endif