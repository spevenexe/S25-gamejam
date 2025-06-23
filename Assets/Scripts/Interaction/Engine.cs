using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

/// <summary>
/// The engine in the engine room. Breaks down, and needs fixing. Only handles logic for breaking and fixing. Does not determine when to break.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class Engine : MonoBehaviour
{
    private AudioSource _engineAmbientAudio;
    [SerializeField] float _breakingVolume;
    [SerializeField] float _timeToFade = 1f;

    private bool isStopped;

    void Awake()
    {
        _engineAmbientAudio = GetComponent<AudioSource>();
    }

    void Start()
    {
        StartEngine();
    }

    /// <summary>
    /// Break the engine.
    /// </summary>
    public void Break()
    {
        if (isStopped)
        {
            Debug.LogWarning("Engine is already stopped.");
            return;
        }

        SFXManager.PlaySound(SFXManager.SoundType.ENGINE_BREAK, _breakingVolume * _engineAmbientAudio.volume);
        StartCoroutine(fadeEngine(_timeToFade));
        isStopped = true;
    }

    /// <summary>
    /// Produces a fading audio whir as the engine breaks.
    /// </summary>
    /// <param name="seconds">the amount of time the fade should last</param>
    /// <returns></returns>
    private IEnumerator fadeEngine(float seconds)
    {
        float originalVolume = _engineAmbientAudio.volume;
        while (seconds > 0)
        {
            _engineAmbientAudio.volume -= _engineAmbientAudio.volume * Time.deltaTime;
            seconds -= Time.deltaTime;
            yield return null;
        }
        _engineAmbientAudio.Stop();
        _engineAmbientAudio.volume = originalVolume;
    }

    /// <summary>
    /// Start the engine.
    /// </summary>
    public void StartEngine()
    {
        if (!_engineAmbientAudio.isPlaying) SFXManager.LoopClip(_engineAmbientAudio);
        isStopped = false;
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