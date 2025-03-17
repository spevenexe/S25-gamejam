using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameTimer : MonoBehaviour
{

    //[Tooltip("This is the amount of seconds it takes to land on the moon.")]
    //[SerializeField] private float _fullGameTime;

    // This records the time
    private float _timer;

    [Tooltip("The second of which an event would start")]
    [SerializeField] private float[] _eventStartTime;

    [Tooltip("The event that would start at Event Start Time")]
    [SerializeField] private EventStarter[] _events;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // increase the timer by deltaTime
        _timer += Time.deltaTime;

        // check if timer passes an eventTime, and if so runs that event
        foreach (float _eventTime in _eventStartTime)
        {
            if (_timer - Time.deltaTime <= _eventTime && _eventTime <= _timer)
            {
                Debug.Log("Event has started.");
            }
        }
    }
}