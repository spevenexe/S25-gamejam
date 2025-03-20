using UnityEngine;

public class MidGame : MonoBehaviour
{

    // mechanic scripts to access
    [SerializeField] private BaseModule engineModule;
    [SerializeField] private HullBreach hullBreachModule;
    [SerializeField] private Navigation navigationModule;

    // timer variables
    [SerializeField] private float _totalMidGameTime;
    private float _timer;
    private float _timerProgress = 0;

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
        if (navigationModule == null) navigationModule = FindAnyObjectByType<Navigation>();
        if (hullBreachModule == null) hullBreachModule = FindAnyObjectByType<HullBreach>();
        if (engineModule == null) engineModule = FindAnyObjectByType<EngineModule>();

        // create values for progression variables
        navigationModule?.SetNavMultipler(_timerProgress);
        hullBreachModule?.SetTimerRanges(_timerProgress);
        engineModule?.SetTimerRanges(_timerProgress);

        // need to make sure engine timer is set

    }

    // Update is called once per frame
    void Update()
    {
        // increase timer by deltaTime
        _timer += Time.deltaTime;

        // update timerProgress
        _timerProgress = _timer / _totalMidGameTime;

        // decrement timer for engineModule
        engineModule?.DecrementTimer(Time.deltaTime);

        // decrement timer for hullBreachModule
        hullBreachModule?.adjustBreachTimer(Time.deltaTime);

        // give timerProgress to each of the modules
        navigationModule?.SetNavMultipler(_timerProgress);
        hullBreachModule?.SetTimerRanges(_timerProgress);
        engineModule?.SetTimerRanges(_timerProgress);

        // end the game when timer up
        if (_timer >= _totalMidGameTime)
        {
            StartEndGame();
            return;
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

    private void StartEndGame()
    {
        // code to end the game, somehow
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
