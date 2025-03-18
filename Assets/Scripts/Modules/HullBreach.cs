using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class HullBreach : MonoBehaviour
{
    [SerializeField] private float _breachStartMaxTime;
    [SerializeField] private float _breachEndMaxTime;
    private float _breachMaxTime;
    [SerializeField] private float _breachMinTime;
    private float _breachTimer;
    private float _breachTimerAveragePercentage = 0.5f;

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
    private void CreateHullBreach()
    {
        throw new NotImplementedException();
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
}
