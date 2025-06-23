using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Handles the summoning of hull breaches.
/// </summary>
public class HullBreachManager : MonoBehaviour
{
    [SerializeField] private float _breachStartMaxTime;
    [SerializeField] private float _breachEndMaxTime;
    private float _breachMaxTime;
    [SerializeField] private float _breachMinTime;
    private float _breachTimer;
    private float _breachTimerBias = 0.5f;

    private Transform [] _breachSpots;
    private HullBreach [] _breaches;
    private HullBreach _breachPrefab;

    void Start()
    {
        GameObject [] objs = GameObject.FindGameObjectsWithTag("Breach Spot");
        if (_breachSpots==null)
        {
            _breachSpots = new Transform[objs.Length];
            for(int i = 0; i < _breachSpots.Length;i++)
                _breachSpots[i] = objs[i].transform;
        }
        _breaches = new HullBreach[_breachSpots.Length];
        _breachPrefab = Resources.Load<HullBreach>("Interactables/Breach");
        StartNewBreachTimer();
    }

    // make sure to call this on start too, to initialize the values
    public void SetTimerRanges(float timerProgress)
    {
        _breachMaxTime = timerProgress * (_breachEndMaxTime - _breachStartMaxTime) + _breachStartMaxTime;
    }

    /// <summary>
    /// Set the time interval until the next breach. Uses chaotic smoothing to vary the spawning time.
    /// </summary>
    private void StartNewBreachTimer()
    {
        // get random percentage
        float breachTimerPercentage = Random.Range(0f, 1f);

        float dist = breachTimerPercentage - _breachTimerBias;
        float weight;

        // weight values towards the bias
        if (dist < 0f)
        {
            weight = breachTimerPercentage / _breachTimerBias;
        }
        else if (dist > 0f)
        {
            // distance ratio
            weight = 1f - ((breachTimerPercentage - _breachTimerBias) / (1f - _breachTimerBias));
        }
        else
        {
            weight = 0f;
        }

        // skew the timer towards the bias to decrease randomness
        float adjustedBreachTimerPercentage = breachTimerPercentage - dist * weight;

        // set breachTimer
        _breachTimer = adjustedBreachTimerPercentage * (_breachMaxTime - _breachMinTime) + _breachMinTime;

        // adjust the bias to the percentage inverse of the random value. In effect, The bias should oscillate between values in a predictable way.
        _breachTimerBias = Mathf.Abs(breachTimerPercentage - 1);
    }

    public void CreateHullBreach()
    {
        List<Transform> openSpots = new List<Transform>();
        // get open spots
        for(int i = 0; i < _breaches.Length; i++)
        {
            // they can have breaches or be ejected
            if(_breaches[i] == null && _breachSpots[i] != null) openSpots.Add(_breachSpots[i]);
        }
        if (openSpots.Count == 0)
        {
            Debug.LogWarning("No open spots");
            return;
        }
        int index =  Random.Range(0,openSpots.Count);
        Transform selectedSpot = openSpots[index];

        //Create breach and add it to list
        HullBreach breach = Instantiate(_breachPrefab, selectedSpot.position,selectedSpot.rotation);
        _breaches[index] = breach;
    }
    
    /// <summary>
    /// Create a hull breach at a given transform
    /// </summary>
    /// <param name="hullBreachTutorialSpot">The transform to create the hull breach at</param>
    internal void CreateHullBreach(Transform hullBreachTutorialSpot)
    {
        Instantiate(_breachPrefab, hullBreachTutorialSpot.position, hullBreachTutorialSpot.rotation);
    }

    // you should call this in Update()
    /// <param name="deltaTime">the amount by which to adjust the timer </param>
    public void AdjustBreachTimer(float deltaTime)
    {
        // decrease breachTimer by deltaTime
        _breachTimer -= deltaTime;
        // if _breachTimer is below 0, create breach and start new timer
        if (_breachTimer <= 0)
        {
            CreateHullBreach();
            StartNewBreachTimer();
        }
    }

    // TODO: this _breaches list is kinda hacky. Might need fixing 
    public int BreachCount()
    {
        int ret = 0;
        foreach (HullBreach b in _breaches)
            ret += (b != null) ? 1 : 0;
        return ret;
    }

    
}

#if UNITY_EDITOR
[CustomEditor(typeof(HullBreachManager))]
public class HullBreachManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.BeginHorizontal();

        EditorGUILayout.LabelField("Time", GUILayout.Width(45f));
        float sec = EditorGUILayout.FloatField(0, GUILayout.Width(45f));

        HullBreachManager hbm = (HullBreachManager) target;
        if(GUILayout.Button("Decrement Timer",GUILayout.Width(120f)))
            hbm.AdjustBreachTimer(100);
        
        GUILayout.EndHorizontal();

    }
}
#endif