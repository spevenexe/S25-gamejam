using System.Linq;
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
    [SerializeField] private MessageDialog beforeNavMessages;
    [SerializeField] private MessageDialog betweenNavAndEngineMessages;
    [SerializeField] private MessageDialog betweenEngineAndHullBreachMessages;
    [SerializeField] private MessageDialog afterHullBreachMessages;

    // private variables
    private int section;
    private bool isFixingModule = false;

    // other variables
    [SerializeField] private AnnouncmentBox announcmentBox;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (navigationModule == null) navigationModule = FindAnyObjectByType<Navigation>();
        if (hullBreachModule == null) hullBreachModule = FindAnyObjectByType<HullBreachManager>();
        if (engineModule == null) engineModule = FindAnyObjectByType<EngineModule>();
    }

    // Update is called once per frame
    void Update()
    {
        // increase section when no messages in Queue or module is fixed
        if (!announcmentBox._messageQueue.Any() && !isFixingModule)
        {
            playAtSection();
            section++;
        }

        // check if modules are fixed
        if (engineModule.IsBroken == false && section == 4)
        {
            isFixingModule = false;
        }
        else if (hullBreachModule.BreachCount() == 0 && section == 6)
        {
            isFixingModule = false;
        }
    }

    private void playAtSection()
    {
        // Debug.Log("did a thing");
        if (section == 0)
        {
            beforeNavMessages.AddMessages();
        }
        if (section == 1)
        {
            teachNavigation();
        }
        if (section == 2)
        {
            betweenNavAndEngineMessages.AddMessages();
        }
        if (section == 3)
        {
            teachEngine();
        }
        if (section == 4)
        {
            betweenEngineAndHullBreachMessages.AddMessages();
        }
        if (section == 5)
        {
            teachHullBreach();
        }
        if (section == 6)
        {
            afterHullBreachMessages.AddMessages();
        }
        if (section >= 7)
        {
            // start midGame
            midGameScript.enabled = true;
            Destroy(this);
        }
    }

    private void teachNavigation()
    {
        Debug.Log("Teaching Navigation");
        isFixingModule = false;
        // don't know if we have nav figured out yet
    }

    private void teachEngine()
    {
        isFixingModule = true;
        engineModule.BreakModule();
    }

    private void teachHullBreach()
    {
        isFixingModule = true;
        hullBreachModule.CreateHullBreach();
    }
}
