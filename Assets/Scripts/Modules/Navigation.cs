using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Navigation : MonoBehaviour
{
    [SerializeField] [Min(0f)] private float _navStartMultiplier;
    [SerializeField] [Min(0f)] private float _navEndMultiplier;
    [SerializeField] [Range(0f,1f)] private float _penaltyMultiplier=0.01f;
    private float _navMultiplier;
    private float _currentPenalty = 0;
    public static bool NagivatedOnce = false;

    [SerializeField] [Min(0f)] private float _timeWithoutNavigationUntilFailure = 20f;
    [SerializeField] private string _warningMessage;
    private bool _sentWarning = false;
    private bool isGamingOver = false;

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

    public void UpdateNavStatus()
    {
        _currentPenalty+=Time.deltaTime;
        if (_currentPenalty >= _timeWithoutNavigationUntilFailure)
        {
            GameOver();
        }else if(_currentPenalty >= _timeWithoutNavigationUntilFailure / 2 && !_sentWarning)
        {
            _sentWarning = true;
            AnnouncmentBox.EnqueueMessage(_warningMessage);
        }
    }

    void GameOver()
    {
        if(isGamingOver) return;
        isGamingOver = true;
        SFXManager.PlaySound(SFXManager.SoundType.CRASH,1f);
        SFXManager.PlaySound(SFXManager.SoundType.ENGINE_BREAK,.4f);
        SFXManager.PlaySound(SFXManager.SoundType.CREEPY_NOISE,.1f);
        LevelLoader.Instance.LoadNext(SceneManager.sceneCountInBuildSettings-1,LevelLoader.TransitionType.FADE);
    }

    public void SetNavMultipler(float timerProgress)
    {
        _navMultiplier = timerProgress * (_navEndMultiplier - _navStartMultiplier) + _navStartMultiplier;
    }

    private void Navigate()
    {
        //While navigating, set nav multiplier to 1 because the ship is on course to the moon
        _currentPenalty = 0f;
        _sentWarning = false;;
        NagivatedOnce = true;
    }

    public void Highlight()
    {
        _monitor.highlight(Color.yellow);
    }

    internal void Init()
    {
        _currentPenalty = 0f;
    }
}
