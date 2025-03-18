using System.Security.Cryptography;
using UnityEngine;

public class MidGame : MonoBehaviour
{

	/*
    FixedEngine() will be called by the engine room when engine is fixed.
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
	private bool _isNavFixed = true;

	// engine mechanic variables
	[SerializeField] private float _engineStartLowRange;
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
    private bool _isLateGame;
    
    // other variables
    public GameObject _owner;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
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
        // increase timer by deltaTime
        _timer += Time.deltaTime;

        // update timerProgress
        _timerProgress = _timer / _totalMidGameTime;

        // decrease engineTimer by deltaTime
        if (_isEngineFixed)
        {
            _engineTimer -= Time.deltaTime;
            // Debug.Log(_engineTimer);
            // if engineTimer is below 0, break engine
            if (_engineTimer <= 0)
            {
                _isEngineFixed = false;
                // Debug.Log("Engine Failure.");
                // break engine code here
            }
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

	private void StartEndGame()
	{
		// future note: change to create gameObject prefab
		EndGame _endGameScript = _owner.AddComponent<EndGame>();
		_endGameScript._owner = _owner;
		Destroy(this);
	}


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
		_breachTimerAveragePercentage = Mathf.Abs(_breachTimerPercentage - 1);

        // Debug.Log(_breachTimer);
        // Debug.Log(_breachNewTimerPercentage);
        // Debug.Log(_breachTimerPercentage);
        // Debug.Log(_breachTimerAveragePercentage);
    }
}
