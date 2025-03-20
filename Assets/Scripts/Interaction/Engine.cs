using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Engine : MonoBehaviour
{
    private AudioSource _engineAmbientAudio;
    [SerializeField] float _breakingVolume;
    [SerializeField] float _timeToFade = 1f;
    
    void Awake()
    {
        _engineAmbientAudio = GetComponent<AudioSource>();
    }

    void Start()
    {
        StartEngine();
    }

    public void Break()
    {
        if (!_engineAmbientAudio.isPlaying)
        {
            Debug.LogWarning("Engine is already stopped.");
            return;
        } 

        SFXManager.PlaySound(SFXManager.SoundType.ENGINE_BREAK,_breakingVolume*_engineAmbientAudio.volume);
        StartCoroutine(fadeEngine(_timeToFade));
    }

    private IEnumerator fadeEngine(float seconds)
    {
        float originalVolume = _engineAmbientAudio.volume;
        while(seconds > 0)
        {
            _engineAmbientAudio.volume-=_engineAmbientAudio.volume*Time.deltaTime;
            seconds-=Time.deltaTime;
            yield return null;
        }
        _engineAmbientAudio.Stop();
        _engineAmbientAudio.volume = originalVolume;
    }

    public void StartEngine() 
    {
        if (!_engineAmbientAudio.isPlaying) SFXManager.LoopClip(_engineAmbientAudio);
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