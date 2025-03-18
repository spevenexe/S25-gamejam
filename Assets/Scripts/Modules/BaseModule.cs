using System.Collections;
using UnityEngine;

public abstract class BaseModule : MonoBehaviour
{
    [SerializeField] protected float _timerStartLowRange,_timerStartHighRange;
    [SerializeField] protected float _timerEndLowRange,_timerEndHighRange;
    protected float _timerLowRange,_timerHighRange;
    protected float _timer;
    public bool IsBroken = false;

    protected abstract void BreakModule();
    protected virtual void FixModule()
    {
        _timer = Random.Range(_timerLowRange, _timerHighRange);
        IsBroken = false;
    }
    protected abstract IEnumerator PlayFixingMinigame();
    public void SetTimerRanges(float timerProgress)
    {
        _timerLowRange = timerProgress * (_timerEndLowRange - _timerStartLowRange) + _timerStartLowRange;
        _timerHighRange = timerProgress * (_timerEndHighRange - _timerStartHighRange) + _timerStartHighRange;
    }
    public void DecrementTimer(float deltaTime)
    {
        // decrease engineTimer by deltaTime
        if (!IsBroken)
        {
            _timer -= Time.deltaTime;
            // Debug.Log(_engineTimer);
            // if engineTimer is below 0, break engine
            if (_timer <= 0)
            {
                IsBroken = true;
                // Debug.Log("Engine Failure.");
                BreakModule();
            }
        }
    }
}
