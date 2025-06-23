using System.Collections;
using UnityEditor;
using UnityEngine;

/// <summary>
/// A module of the ship. Spawns events at certain intervals.
/// </summary>
public abstract class BaseModule : MonoBehaviour
{
    [SerializeField] protected float _timerStartLowRange, _timerStartHighRange;
    [SerializeField] protected float _timerEndLowRange, _timerEndHighRange;
    protected float _timerLowRange, _timerHighRange;
    protected float _timer = 1000;
    public bool IsBroken { get; private set; } = false;

    /// <summary>
    /// The lights of the module. Used to indicate a breakage.
    /// </summary>
    private ModuleLights _moduleLights;
    [SerializeField] protected float _volume = 1f;
    [SerializeField] protected float _alarmInterval = 1.5f;
    [SerializeField] protected float _lightFlashInterval = 1.5f;
    [SerializeField] protected SFXManager.ALARM_INTENSITY _alarmIntensity = SFXManager.ALARM_INTENSITY.LOW;

    [SerializeField] private Transform breachGroup;
    private HullBreach[] breaches;

    // Ejection stuff
    [SerializeField][Min(0f)] private float _randPosOffset = 100f;
    [SerializeField][Min(0f)] private float _randRotationOffset = 180f;
    [SerializeField][Min(0f)] private float _ejectTime = 10f;
    public float EjectTime { get => _ejectTime / 2 + _shakeTime; }
    [SerializeField][Min(0f)] private float _shakeTime = 5f;
    [SerializeField][Min(0f)] private float _jitter = .1f;
    [SerializeField]private BoxCollider _invisibleWall;
    [SerializeField] private Door _door;

    protected virtual void Start()
    {
        // initialize the timers and configure the state of the object
        _timer = Random.Range(_timerStartLowRange, _timerStartHighRange);
        IsBroken = false;
        _moduleLights = GetComponentInChildren<ModuleLights>();
        breaches = GetComponentsInChildren<HullBreach>();
        _invisibleWall.enabled = false;
    }

    // TODO breaking can be done as an event. This can decouple a lot of invocations and make coding smoother.
    public virtual void BreakModule()
    {
        // set a public variable flag, which MidGame.cs can read to indicate its broken
        IsBroken = true;

        StartCoroutine(FlashAlarmLights());
        StartCoroutine(LoopAlarmSound(_alarmIntensity, _alarmInterval, _volume));
        // play the minigame to fix module
        StartCoroutine(PlayFixingMinigame());
    }

    /// <summary>
    /// Repeatedly flash the module lights using the alarm color.
    /// </summary>
    private IEnumerator FlashAlarmLights()
    {
        _moduleLights.SetAlarmColor();
        yield return new WaitForSeconds(_lightFlashInterval);
        while (IsBroken)
        {
            _moduleLights.TurnOff();
            yield return new WaitForSeconds(_lightFlashInterval);
            if (IsBroken) _moduleLights.RestoreColor(); // a second check in case the module gets fixed
            yield return new WaitForSeconds(_lightFlashInterval);
        }
        _moduleLights.ResetColorDefault();
    }

    private IEnumerator LoopAlarmSound(SFXManager.ALARM_INTENSITY alarmIntensity, float alarmInterval, float volume)
    {
        yield return new WaitForSeconds(alarmInterval);
        while (IsBroken)
        {
            AudioClip clip = SFXManager.GetAudioClipAt(SFXManager.SoundType.ALARM, (int)alarmIntensity);
            SFXManager.PlayClip(clip, volume);
            yield return new WaitForSeconds(alarmInterval + clip.length);
        }
    }

    protected virtual void FixModule()
    {
        _moduleLights.ResetColorDefault();
        IsBroken = false;
        _timer = Random.Range(_timerLowRange, _timerHighRange);
    }
    protected abstract IEnumerator PlayFixingMinigame();

    // TODO: everything here is calculated, every frame. Is there even a point to this function?
    /// <summary>
    /// Set the bounds of <c>_timerLowRange</c> and <c>_timerHighRange</c>. The random timer interval always falls between these two, as they shrink
    /// </summary>
    /// <param name="timerProgress"></param>
    public void SetTimerRanges(float timerProgress)
    {
        _timerLowRange = timerProgress * (_timerEndLowRange - _timerStartLowRange) + _timerStartLowRange;
        _timerHighRange = timerProgress * (_timerEndHighRange - _timerStartHighRange) + _timerStartHighRange;
    }

    /// <summary>
    /// Set a random timer interval between <c>_timerLowRange</c> and <c>_timerHighRange</c>.
    /// </summary>
    public void InitTimer()
    {
        _timer = Random.Range(_timerLowRange, _timerHighRange);
    }

    /// <summary>
    /// Decrement the timer of the module and break it if the timer completes. 
    /// </summary>
    /// <param name="seconds">The amount of time to decrease the timer by</param>
    public virtual void DecrementTimer(float seconds)
    {
        if (!IsBroken)
        {
            _timer -= seconds;
            if (_timer <= 0)
            {
                BreakModule();
            }
        }
    }

    public void EjectModule() => StartCoroutine(EjectHelper());

    private IEnumerator EjectHelper()
    {
        // game logic
        foreach (HullBreach b in breaches)
            Destroy(b.gameObject);

        PInput player = FindAnyObjectByType<PInput>();

        // Visually Break Module for some flair
        IsBroken = true;
        _moduleLights.SetAlarmColor();
        StartCoroutine(LoopAlarmSound(_alarmIntensity, _alarmInterval, _volume));
        StartCoroutine(FlashAlarmLights());

        // violently shake the module
        GetComponent<MeshCollider>().excludeLayers = LayerMask.GetMask("Interactable");

        Vector3 basePos = transform.position;
        for (float timer = 0; timer < _shakeTime ||
            _invisibleWall.enabled == false;
            timer += Time.deltaTime)
        {

            transform.position = basePos + new Vector3(
                Random.Range(-_jitter, _jitter),
                Random.Range(-_jitter, _jitter)
            );

            if (player.transform.position.z > _invisibleWall.gameObject.transform.position.z)
            {
                _invisibleWall.transform.SetParent(null);
                _invisibleWall.enabled = true;
            }

            yield return null;
        }

        // actually eject module
        Vector3 initPos = transform.position;
        Vector3 destPos = initPos;
        destPos.x += Random.Range(-_randPosOffset, _randPosOffset);
        destPos.y += Random.Range(-_randPosOffset, _randPosOffset);
        destPos.z -= Random.Range(0, _randPosOffset * 2);

        Vector3 initAngles = transform.eulerAngles;
        Vector3 destAngles = initAngles;
        _randRotationOffset *= 20; // this lets the rotation spin like crazy
        destAngles.x += Random.Range(-_randRotationOffset, _randRotationOffset);
        destAngles.y += Random.Range(-_randRotationOffset, _randRotationOffset);
        destAngles.z += Random.Range(-_randRotationOffset, _randRotationOffset);
        Quaternion initRotation = transform.rotation;
        Quaternion destRotation = Quaternion.Euler(destAngles);
        for (float timer = 0; timer < _ejectTime; timer += Time.deltaTime)
        {
            float t = timer / (_ejectTime - timer);
            transform.position = Vector3.Lerp(initPos, destPos, t);
            transform.rotation = Quaternion.Slerp(initRotation, destRotation, t);

            if (timer > _ejectTime / 2)
            {
                FixModule();
                _moduleLights.SetAlarmColor();
                // close the next module's door
                _door.Close();
            }

            yield return null;
        }
        // delete module
        Destroy(gameObject);
        yield break;
    }
}


#if UNITY_EDITOR
[CustomEditor(typeof(BaseModule),true)]
public class BaseModuleInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();


        GUILayout.BeginHorizontal();

        EditorGUILayout.LabelField("Time", GUILayout.Width(45f));
        float sec = EditorGUILayout.FloatField(0, GUILayout.Width(45f));

        BaseModule baseModule = (BaseModule) target;
        if (GUILayout.Button("Decrement Timer", GUILayout.Width(120f)))
            baseModule.DecrementTimer(sec);
        
        GUILayout.EndHorizontal();
        
        if (GUILayout.Button("Break Module", GUILayout.Width(120f)))
            baseModule.BreakModule();

        if(GUILayout.Button("Eject Module",GUILayout.Width(120f)))
            baseModule.EjectModule();
            
    }
}
#endif