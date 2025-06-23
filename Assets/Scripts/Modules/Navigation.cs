using System;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Navigation room. Slowly veers off course over time, must be corrected.
/// <br/>
/// TODO: turn navigation into a minigame.
/// </summary>
public class Navigation : MonoBehaviour
{
    [SerializeField][Min(0f)] private float _navStartMultiplier;
    [SerializeField][Min(0f)] private float _navEndMultiplier;
    [SerializeField][Range(0f, 1f)] private float _penaltyMultiplier = 0.01f;
    private float _navMultiplier;
    private float _currentPenalty = 0;
    public static bool NagivatedOnce = false;

    [SerializeField][Min(0f)] private float _timeWithoutNavigationUntilFailure = 20f;
    [SerializeField] private string _warningMessage;
    private bool _sentWarning = false;
    private bool isGamingOver = false;

    // when this value is large, the penalty is worse. Subtract this from progress
    // the fancy math prevents underflow from multiplying very small numbers
    public float OffCoursePenalty
    {
        get
        {

            float logSum = Mathf.Log10(_currentPenalty) + Mathf.Log10(_penaltyMultiplier) + Mathf.Log10(_navMultiplier);
            return Mathf.Pow(10, logSum);
        }
    }

    [SerializeField] private Monitor _monitor;

    void OnEnable()
    {
        if (_monitor) _monitor.InteractionTriggers += Navigate;
    }

    void OnDisable()
    {
        if (_monitor) _monitor.InteractionTriggers -= Navigate;
    }

    /// <summary>
    /// Increase the navigation penalty, dropping nav speed
    /// </summary>
    public void UpdateNavStatus()
    {
        _currentPenalty += Time.deltaTime;
        if (_currentPenalty >= _timeWithoutNavigationUntilFailure)
        {
            GameOver();
        }
        // Warn the player to navigate
        else if (_currentPenalty >= _timeWithoutNavigationUntilFailure / 2 && !_sentWarning)
        {
            _sentWarning = true;
            AnnouncmentBox.EnqueueMessage(_warningMessage);
        }
    }

    // TODO: this could probably be improved
    void GameOver()
    {
        if (isGamingOver) return; // we do this to lock out recurring calls to GameOver
        isGamingOver = true;
        SFXManager.PlaySound(SFXManager.SoundType.CRASH, 1f);
        SFXManager.PlaySound(SFXManager.SoundType.ENGINE_BREAK, .4f);
        SFXManager.PlaySound(SFXManager.SoundType.CREEPY_NOISE, .1f);
        LevelLoader.Instance.LoadNext(SceneManager.sceneCountInBuildSettings - 1, LevelLoader.TransitionType.FADE);
    }

    /// <summary>
    /// Set the penalty multiplier. The further the game progresses, the more punishing navigation becomes.
    /// </summary>
    /// <param name="timerProgress">The progress of the game</param>
    public void SetNavMultipler(float timerProgress)
    {
        _navMultiplier = timerProgress * (_navEndMultiplier - _navStartMultiplier) + _navStartMultiplier;
    }

    /// <summary>
    /// Re-align the ship. Removes navigation penalty.
    /// </summary>
    private void Navigate()
    {
        //While navigating, set nav multiplier to 1 because the ship is on course to the moon
        _currentPenalty = 0f;
        _sentWarning = false; ;
        NagivatedOnce = true;
    }

    public void Highlight()
    {
        _monitor.Highlight(Color.yellow);
    }

    /// <summary>
    /// Initialize the module. Call with manager scripts to ensure variables are initialized.
    /// </summary>
    internal void Init()
    {
        _currentPenalty = 0f;
    }
}
