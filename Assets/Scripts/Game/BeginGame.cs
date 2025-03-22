using UnityEngine;

public class BeginGame : MonoBehaviour
{
    // midGame script
    [SerializeField] private MidGame midGameScript;

    // module scripts to access
    [SerializeField] private EngineModule engineModule;
    [SerializeField] private HullBreachManager hullBreachModule;
    [SerializeField] private Navigation navigationModule;

    // serialized variables
    [SerializeField] private string[] messages;
    [SerializeField] private int teachNavigationPausePoint;
    [SerializeField] private int teachEnginePausePoint;
    [SerializeField] private int teachHullBreachPausePoint;
    [SerializeField] private float timeBetweenMessages;

    // private variables
    private float timer;
    private int section;
    private bool isFixingModule;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (navigationModule == null) navigationModule = FindAnyObjectByType<Navigation>();
        if (hullBreachModule == null) hullBreachModule = FindAnyObjectByType<HullBreachManager>();
        if (engineModule == null) engineModule = FindAnyObjectByType<EngineModule>();

        // set timer first
        timer = timeBetweenMessages;
    }

    // Update is called once per frame
    void Update()
    {
        // decrement timer
        if (!isFixingModule)
        {
            timer -= Time.deltaTime;
        }

        // increase section when timer = 0
        if (timer <= 0)
        {
            timer = timeBetweenMessages;
            playMessage();
            if (section == teachNavigationPausePoint)
            {
                isFixingModule = true;
                teachNavigation();
            }
            else if (section == teachEnginePausePoint)
            {
                isFixingModule = true;
                teachEngine();
            }
            else if (section == teachHullBreachPausePoint)
            {
                isFixingModule = true;
                teachHullBreach();
            }
            section++;
        }

        // if run out of messages, start MidGame
        if (section >= messages.Length)
        {
            midGameScript.enabled = true;
            Destroy(this);
        }

    }

    private void playMessage()
    {
        Debug.Log(messages[section]);
    }

    private void teachNavigation()
    {
        Debug.Log("Teaching Navigation");
        isFixingModule = false;
    }

    private void teachEngine()
    {
        Debug.Log("Teaching Engine");
        isFixingModule = false;
    }

    private void teachHullBreach()
    {
        Debug.Log("Teaching Hull Breaches");
        isFixingModule = false;
    }
}
