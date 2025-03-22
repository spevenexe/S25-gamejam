using System;
using UnityEngine;

public class Navigation : MonoBehaviour
{
    [SerializeField] [Min(0f)] private float _navStartMultiplier;
    [SerializeField] [Min(0f)] private float _navEndMultiplier;
    [SerializeField] [Range(0f,1f)] private float _penaltyMultiplier=0.01f;
    private float _navMultiplier;
    private float _currentPenalty = 0;
    public static bool NagivatedOnce = false;

    // when this value is large, the penalty is worse. Subtract this from progress
    // the fancy math prevents underflow from multiplying very small numbers
    public float OffCoursePenalty{
        get
        {
            
            float logSum = Mathf.Log10(_currentPenalty)+Mathf.Log10(_penaltyMultiplier)+Mathf.Log10(_navMultiplier);
            return Mathf.Pow(10,logSum);
        }
    }

    [SerializeField] private Monitor _monitor;

    void Start()
    {
        if (_monitor) _monitor.InteractionTriggers+=Navigate;
    }

    void OnDestroy()
    {
        _monitor.InteractionTriggers-=Navigate;
    }

    void Update()
    {
        // _currentPenalty=Mathf.Min(1f,_currentPenalty+Time.deltaTime);
        _currentPenalty+=Time.deltaTime;
    }

    public void SetNavMultipler(float timerProgress)
    {
        _navMultiplier = timerProgress * (_navEndMultiplier - _navStartMultiplier) + _navStartMultiplier;
    }

    private void Navigate()
    {
        //While navigating, set nav multiplier to 1 because the ship is on course to the moon
        _currentPenalty = 0f;

        NagivatedOnce = true;
    }

    public void Highlight()
    {
        _monitor.highlight(Color.yellow);
    } 
}
