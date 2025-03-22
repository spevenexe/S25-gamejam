using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class HullBreachManager : MonoBehaviour
{
    [SerializeField] private float _breachStartMaxTime;
    [SerializeField] private float _breachEndMaxTime;
    private float _breachMaxTime;
    [SerializeField] private float _breachMinTime;
    private float _breachTimer;
    private float _breachTimerAveragePercentage = 0.5f;

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
    }

    // make sure to call this on start too, to initialize the values
    public void SetTimerRanges(float timerProgress)
    {
        _breachMaxTime = timerProgress * (_breachEndMaxTime - _breachStartMaxTime) + _breachStartMaxTime;
    }

    private void StartNewBreachTimer()
    {
        // get random percentage
        float _breachTimerPercentage = Random.Range(0f, 1f);

        // create additional variables
        float _breachTimerPercentageOfAverageTimer = 0f;
        float _breachTimerPercentageToAdd = 0f;
        float _breachNewTimerPercentage = 0f;

        // adjust breachTimerPercentage based on breachTimerAveragePercentage
        if (_breachTimerPercentage < _breachTimerAveragePercentage)
        {
            _breachTimerPercentageOfAverageTimer = _breachTimerPercentage / _breachTimerAveragePercentage;
            _breachTimerPercentageToAdd = _breachTimerPercentageOfAverageTimer * (_breachTimerAveragePercentage - _breachTimerPercentage);
            _breachNewTimerPercentage = _breachTimerPercentage + _breachTimerPercentageToAdd;
        }
        else if (_breachTimerPercentage > _breachTimerAveragePercentage)
        {
            _breachTimerPercentageOfAverageTimer = Mathf.Abs((_breachTimerPercentage - _breachTimerAveragePercentage) / (1 - _breachTimerAveragePercentage) - 1);
            _breachTimerPercentageToAdd = -(_breachTimerPercentageOfAverageTimer * (_breachTimerPercentage - _breachTimerAveragePercentage));
            _breachNewTimerPercentage = _breachTimerPercentage + _breachTimerPercentageToAdd;
        }
        else
        {
            _breachNewTimerPercentage = _breachTimerPercentage;
        }

        // set breachTimer with breachNewTimerPercentage
        _breachTimer = _breachNewTimerPercentage * (_breachMaxTime - _breachMinTime) + _breachMinTime;

        // adjust breachTimerAveragePercentage to new average
        _breachTimerAveragePercentage = Mathf.Abs(_breachTimerPercentage-1);

        // Debug.Log(_breachTimer);
        // Debug.Log(_breachNewTimerPercentage);
        // Debug.Log(_breachTimerPercentage);
        // Debug.Log(_breachTimerAveragePercentage);
    }

    // Keianna TODO
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
    

    // you should call this in Update()
    public void adjustBreachTimer(float deltaTime)
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

    public int BreachCount()
    {
        int ret = 0;
        foreach(HullBreach b in _breaches)
            ret+= (b != null) ? 1 : 0;
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
            hbm.adjustBreachTimer(100);
        
        GUILayout.EndHorizontal();

    }
}
#endif