using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public abstract class BaseModule : MonoBehaviour
{
    [SerializeField] protected float _timerStartLowRange,_timerStartHighRange;
    [SerializeField] protected float _timerEndLowRange,_timerEndHighRange;
    protected float _timerLowRange,_timerHighRange;
    protected float _timer;
    public bool IsBroken {get; private set;} = false;

    private ModuleLights _moduleLights;
    [SerializeField] protected float _volume=1f;
    [SerializeField] protected float _alarmInterval = 1.5f; 
    [SerializeField] protected SFXManager.ALARM_INTENSITY _alarmIntensity = SFXManager.ALARM_INTENSITY.LOW; 

    void Start(){
        _timer = Random.Range(_timerLowRange, _timerHighRange);
        IsBroken = false;
        _moduleLights = GetComponentInChildren<ModuleLights>();
    }

    protected virtual void BreakModule()
    {        
        // set a public variable flag, which MidGame.cs can read to indicate its broken
        IsBroken = true;

        _moduleLights.SetAlarmColor();

        // play the minigame to fix module
        StartCoroutine(PlayAlarm(_alarmIntensity,_alarmInterval,_volume));
        StartCoroutine(PlayFixingMinigame());
    }

    private IEnumerator PlayAlarm(SFXManager.ALARM_INTENSITY alarmIntensity, float alarmInterval, float volume)
    {
        yield return new WaitForSeconds(alarmInterval);
        while(IsBroken)
        {
            AudioClip clip = SFXManager.GetAudioClipAt(SFXManager.SoundType.ALARM,(int)alarmIntensity);
            SFXManager.PlayClip(clip,volume);
            yield return new WaitForSeconds(alarmInterval+clip.length);
        }
    }

    protected virtual void FixModule()
    {
        _moduleLights.ResetColor();
        IsBroken = false;
        _timer = Random.Range(_timerLowRange, _timerHighRange);
    }
    protected abstract IEnumerator PlayFixingMinigame();
    public void SetTimerRanges(float timerProgress)
    {
        _timerLowRange = timerProgress * (_timerEndLowRange - _timerStartLowRange) + _timerStartLowRange;
        _timerHighRange = timerProgress * (_timerEndHighRange - _timerStartHighRange) + _timerStartHighRange;
    }
    public void DecrementTimer(float seconds)
    {
        // decrease engineTimer by deltaTime
        if (!IsBroken)
        {
            _timer -= seconds;
            // Debug.Log(_engineTimer);
            // if engineTimer is below 0, break engine
            if (_timer <= 0)
            {
                BreakModule();
            }
        }
    }
}


#if UNITY_EDITOR
[CustomEditor(typeof(BaseModule),true)]
public class BaseModuleInspector : Editor
{
    // private SerializedProperty secondsProperty;

    // private void OnEnable()
    // {
    //     secondsProperty = serializedObject.FindProperty("SecondsToDecrement");
    // }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();


        // float sec = secondsProperty.floatValue; 
        GUILayout.BeginHorizontal();

        EditorGUILayout.LabelField("Time", GUILayout.Width(45f));
        float sec = EditorGUILayout.FloatField(0, GUILayout.Width(45f));
        // EditorGUILayout.PropertyField(secondsProperty);

        BaseModule baseModule = (BaseModule) target;
        if(GUILayout.Button("Decrement Timer",GUILayout.Width(120f)))
            baseModule.DecrementTimer(sec);
        
        GUILayout.EndHorizontal();

    }
}
#endif