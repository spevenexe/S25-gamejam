using System.Security.Cryptography;
using UnityEngine;

public class MidGame : MonoBehaviour
{

    /*
    FixedEngine() will be called by the engine room when engine is fixed.
<<<<<<< HEAD
=======
    ModuleEjected() will be called when a module gets ejected.
>>>>>>> 98982bf0a4e34160adef4c4ef243afe57efc91f1
    */

    // mechanic scripts to access
    // [SerializeField] private ScriptMechanic _scriptMechanic

    // timer variables
    [SerializeField] private float _totalMidGameTime;
    [HideInInspector] public float _timer;
    private float _timerProgress = 0;

    // navigation mechanic variables
    [SerializeField] private float _navStartMultiplier;
    [SerializeField] private float _navEndMultiplier;
    private float _navMultiplier;
<<<<<<< HEAD
    private bool _isNavFixed = true;

	// engine mechanic variables
	[SerializeField] private float _engineStartLowRange;
=======

    // engine mechanic variables
    [SerializeField] private float _engineStartLowRange;
>>>>>>> 98982bf0a4e34160adef4c4ef243afe57efc91f1
    [SerializeField] private float _engineStartHighRange;
    [SerializeField] private float _engineEndLowRange;
    [SerializeField] private float _engineEndHighRange;
    private float _engineLowRange;
    private float _engineHighRange;
    private float _engineTimer;
    private bool _isEngineFixed = true;

    // hull breach mechanic variables
    [SerializeField] private float _breachStartMaxTime;
    [SerializeField] private float _breachEndMaxTime;
    private float _breachMaxTime;
    [SerializeField] private float _breachMinTime;
    private float _breachTimer;
    private float _breachTimerAveragePercentage = 0.5f;

    // game state variables (not used yet)
    private bool _isEarlyGame;
<<<<<<< HEAD
    private bool _isLateGame;
    
    // other variables
    public GameObject _owner;

	//time to reach moon should decrease as the game progresses
	private float _timeToReachMoon;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
=======
    private bool _isLateGame = false;

    // late game variables
    [SerializeField] private float _maxTimeBetweenEjections;
    [SerializeField] private float _minTimeBetweenEjections;
    private float _ejectionTimer;
    private bool _isEjectionTimerRunning = false;

    // other variables
    public GameObject _owner;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
>>>>>>> 98982bf0a4e34160adef4c4ef243afe57efc91f1
    {
        // create values for progression variables
        NavProgression();
        EngineProgression();
        HullBreachProgression();

        // start engine fixed
        FixedEngine();

    }

    // Update is called once per frame
    void Update()
    {
<<<<<<< HEAD
        // increase timer by deltaTime
        _timer += Time.deltaTime;

        // update timerProgress
        _timerProgress = _timer / _totalMidGameTime;
=======
        // don't update if isLateGame
        if (!_isLateGame)
        {
            // increase timer by deltaTime
            _timer += Time.deltaTime;

            // update timerProgress
            _timerProgress = _timer / _totalMidGameTime;
        }
>>>>>>> 98982bf0a4e34160adef4c4ef243afe57efc91f1

        // decrease engineTimer by deltaTime
        if (_isEngineFixed)
        {
            _engineTimer -= Time.deltaTime;
            // Debug.Log(_engineTimer);
            // if engineTimer is below 0, break engine
            if (_engineTimer <= 0)
            {
<<<<<<< HEAD
                // Debug.Log("Engine Failure.");
                // break engine code here
                BreakEngine();      

            }
        }

		//if engine is broken, time to reach moon should remain constant
		if (!_isEngineFixed)
        {
            timeToReachMoon = _timeToReachMoon;
        }
		//if nav is broken, time to reach moon should increase
		else if (_isNavFixed)
        {
			timeToReachMoon = _timeToReachMoon * (1 + _timerProgress);

		}
		//if nav is fixed, time to reach moon should decrease
		else
		{
            timeToReachMoon = _timeToReachMoon * (1 - _timerProgress);
        }

			// decrease breachTimer by deltaTime
			_breachTimer -= Time.deltaTime;
        // if _breachTimer is below 0, create breach and start new timer
        if (_breachTimer <= 0)
        {
            // code to create hull breach
            StartNewBreachTimer();
        }

        // run mechanic progressions
        NavProgression();
        EngineProgression();
        HullBreachProgression();

        // start EndGame
        if (_timer >= _totalMidGameTime)
        {
            StartEndGame();
=======
                _isEngineFixed = false;
                // Debug.Log("Engine Failure.");
                BreakEngine();
            }
        }

        // decrease breachTimer by deltaTime
        _breachTimer -= Time.deltaTime;
        // if _breachTimer is below 0, create breach and start new timer
        if (_breachTimer <= 0)
        {
            CreateHullBreach();
            StartNewBreachTimer();
        }

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

        // don't update if isLateGame
        if (!_isLateGame)
        {
            // run mechanic progressions
            NavProgression();
            EngineProgression();
            HullBreachProgression();
        }

        // start LateGame
        if (_timer >= _totalMidGameTime && !_isLateGame)
        {
            StartLateGame();
>>>>>>> 98982bf0a4e34160adef4c4ef243afe57efc91f1
        }
        
    }

    private void NavProgression()
    {
        // update navMultiplier
        _navMultiplier = _timerProgress * (_navEndMultiplier - _navStartMultiplier) + _navStartMultiplier;
        // Debug.Log(_navMultiplier);
    }

    private void EngineProgression()
    {
        // update engineRanges
        _engineLowRange = _timerProgress * (_engineEndLowRange - _engineStartLowRange) + _engineStartLowRange;
        _engineHighRange = _timerProgress * (_engineEndHighRange - _engineStartHighRange) + _engineStartHighRange;
        // Debug.Log(_engineLowRange);
        // Debug.Log(_engineHighRange);

    }

    private void HullBreachProgression()
    {
        // update breachMaxTime
        _breachMaxTime = _timerProgress * (_breachEndMaxTime - _breachStartMaxTime) + _breachStartMaxTime;
        // Debug.Log(_breachMaxTime);
    }

    public void FixedEngine()
    {
        _engineTimer = Random.Range(_engineLowRange, _engineHighRange);
        _isEngineFixed = true;
    }

<<<<<<< HEAD
=======
    public void ModuleEjected()
    {
        StartEjectionTimer();
    }

    private void StartLateGame()
    {
        _isLateGame = true;
        StartEjectionTimer();
    }

>>>>>>> 98982bf0a4e34160adef4c4ef243afe57efc91f1
    private void StartEndGame()
    {
        // future note: change to create gameObject prefab
        EndGame _endGameScript = _owner.AddComponent<EndGame>();
        _endGameScript._owner = _owner;
        Destroy(this);
    }

<<<<<<< HEAD

    //When Engine is broken time to reach the moon should remain constant
    private void BreakEngine()
	{
		_isEngineFixed = false;



	}
	//When nav is broken time to reach the moon should increase
	private void BreakNav()
	{
		// future note: add nav mechanic script to access
		// _scriptMechanic._isNavFixed = false;
	}

    //When eletrical is broken the display should be dimmer
    private void BreakElectrical()
    {
		// future note: add electrical mechanic script to access
		// _scriptMechanic._isElectricalFixed = false;

	}

	private void StartNewBreachTimer()
=======
    private void StartNewBreachTimer()
>>>>>>> 98982bf0a4e34160adef4c4ef243afe57efc91f1
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
<<<<<<< HEAD
=======

    private void StartEjectionTimer()
    {
        _isEjectionTimerRunning = true;
        _ejectionTimer = Random.Range(_minTimeBetweenEjections, _maxTimeBetweenEjections);
    }

    public void BreakEngine()
    {
        // break engine code here
    }

    public void CreateHullBreach()
    {
        // create hull breach code here
    }

    public void StartEjectionProblem()
    {
        // start ejection problem code here
    }
>>>>>>> 98982bf0a4e34160adef4c4ef243afe57efc91f1
}
