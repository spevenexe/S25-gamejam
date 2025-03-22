using System;
using System.Collections;
using UnityEngine;

public class MidGame : MonoBehaviour
{

    // mechanic scripts to access
    [SerializeField] private EngineModule engineModule;
    [SerializeField] private HullBreachManager hullBreachModule;
    [SerializeField] private Navigation navigationModule;
    [SerializeField] private Window _window;

    [Tooltip("This should NOT contain the engine module")]
    [SerializeField] private BaseModule [] _modules;

    // timer variables
    [SerializeField] private float _totalMidGameTime;
    [SerializeField] private float _breachTimePenalty=1;
    private float _timer = 0;
    private float _timerProgress = 0;

    [SerializeField] private ProgressBar _progressBar;

    /*
    // game state variables (not used yet)
    private bool _isEarlyGame;
    private bool _isLateGame = false;

    // late game variables
    [SerializeField] private float _maxTimeBetweenEjections;
    [SerializeField] private float _minTimeBetweenEjections;
    private float _ejectionTimer;
    private bool _isEjectionTimerRunning = false;
    */

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(_modules == null || _modules.Length == 0) throw new Exception("No modules found");
        if (navigationModule == null) navigationModule = FindAnyObjectByType<Navigation>();
        if (hullBreachModule == null) hullBreachModule = FindAnyObjectByType<HullBreachManager>();
        if (engineModule == null) engineModule = FindAnyObjectByType<EngineModule>();
        if (_window == null) _window = FindAnyObjectByType<Window>();
        if (_progressBar == null) _progressBar = FindAnyObjectByType<ProgressBar>();

        // create values for progression variables
        navigationModule?.SetNavMultipler(_timerProgress);
        hullBreachModule?.SetTimerRanges(_timerProgress);
        engineModule?.SetTimerRanges(_timerProgress);

        // need to make sure engine timer is set
        _timer = 0;
    }

    void OnEnable()
    {
        navigationModule?.SetNavMultipler(_timerProgress);
        navigationModule?.Init();
        hullBreachModule?.SetTimerRanges(_timerProgress);
        engineModule?.SetTimerRanges(_timerProgress);
        engineModule?.InitTimer();


        foreach(BaseModule bm in _modules)
        {
            bm?.SetTimerRanges(_timerProgress);
            bm?.InitTimer();
        }

        _timer = 0;
        _timerProgress = 0;
        _progressBar.gameObject.SetActive(true);
        _progressBar.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        // increase timer by deltaTime, only if the engine is working
        if(!engineModule.IsBroken)
        {
            float newTime = _timer + Time.deltaTime - navigationModule.OffCoursePenalty;
            _timer = Mathf.Max(newTime,_timer);
        }

        // update timerProgress
        _timerProgress = _timer / _totalMidGameTime;
        _window.SetScale(_timerProgress);
        _progressBar.SetProgress(_timerProgress);

        // decrement timers
        navigationModule?.UpdateNavStatus();
        engineModule?.DecrementTimer(Time.deltaTime);
        hullBreachModule?.adjustBreachTimer(Time.deltaTime);
        foreach(BaseModule bm in _modules)
            bm?.DecrementTimer(Time.deltaTime);


        // give timerProgress to each of the modules
        navigationModule?.SetNavMultipler(_timerProgress);
        hullBreachModule?.SetTimerRanges(_timerProgress);
        engineModule?.SetTimerRanges(_timerProgress);
        foreach(BaseModule bm in _modules)
            bm?.SetTimerRanges(_timerProgress);

        // end the game when timer up
        if (_timer >= _totalMidGameTime)
        {
            StartEndGame();
            Debug.Log("Midgame complete");
        }

        /*
        // decrease ejectionTimer by deltaTime
        if (_isEjectionTimerRunning)
        {
            _ejectionTimer -= Time.deltaTime;
            // if _ejectionTime is below 0, stop timer and start ejection problem
            if (_ejectionTimer <= 0)
            {
                _isEjectionTimerRunning = false;
                StartEjectionProblem();
            }
        }
        */

    }

    private void StartEndGame() => StartCoroutine(StartEndGameHelper());

    private IEnumerator StartEndGameHelper()
    {
        EndGame endGame = Instantiate(Resources.Load<EndGame>("EndGame"),null);
        // make sure to pass the ejection order to the endgame
        while(!endGame.didAwake) yield return null;
        endGame.Modules.Add(engineModule);
        endGame.Modules.AddRange(_modules);

        enabled = false;
        yield break;
    }

    /*
    public void ModuleEjected()
    {
        StartEjectionTimer();
    }

    private void StartLateGame()
    {
        _isLateGame = true;
        StartEjectionTimer();
    }

    private void StartEjectionTimer()
    {
        _isEjectionTimerRunning = true;
        _ejectionTimer = Random.Range(_minTimeBetweenEjections, _maxTimeBetweenEjections);
    }

    public void StartEjectionProblem()
    {
        // start ejection problem code here
    }
    */
}
