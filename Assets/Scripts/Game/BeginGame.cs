using System;
using System.Collections;
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
    [SerializeField] private MessageDialog teachNavMessages;
    [SerializeField] private MessageDialog betweenNavAndEngineMessages;
    [SerializeField] private MessageDialog teachEngineMessages;
    [SerializeField] private MessageDialog betweenEngineAndHullBreachMessages;
    [SerializeField] private MessageDialog teachHullBreachMessages;
    [SerializeField] private MessageDialog afterHullBreachMessages;

    [SerializeField] private Transform hullBreachTutorialSpot;
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
        if (Navigation.NagivatedOnce && section == 2)
        {
            isFixingModule = false;
        }
        else if (engineModule.IsBroken == false && section == 4)
        {
            isFixingModule = false;
        }
        else if (HullBreach.BreachDestroyedOnce && section == 6)
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
            AnnouncmentBox.ClearMessages();
            betweenNavAndEngineMessages.AddMessages();
        }
        if (section == 3)
        {
            teachEngine();
        }
        if (section == 4)
        {
            AnnouncmentBox.ClearMessages();
            betweenEngineAndHullBreachMessages.AddMessages();
        }
        if (section == 5)
        {
            teachHullBreach();
        }
        if (section == 6)
        {
            AnnouncmentBox.ClearMessages();
            afterHullBreachMessages.AddMessages();
        }
        if (section >= 7)
        {
            // start midGame
            StartMidGame();
        }
    }

    private void StartMidGame() => StartCoroutine(StartMidGameHelper());

    private IEnumerator StartMidGameHelper()
    {
        yield return new WaitForSeconds(10f);
        midGameScript.enabled = true;
        Destroy(this);
    }

    private void teachNavigation()
    {
        teachNavMessages.AddMessages();
        isFixingModule = true; 
        Navigation.NagivatedOnce = false;
        // kind of spaghetti code but its being handled in the nav module class
    }

    public void FixNavigation() => isFixingModule = false;

    private void teachEngine()
    {
        teachEngineMessages.AddMessages();
        isFixingModule = true;
        engineModule.BreakModule();
    }

    private void teachHullBreach()
    {
        teachHullBreachMessages.AddMessages();
        isFixingModule = true;
        HullBreach.BreachDestroyedOnce = false;
        hullBreachModule.CreateHullBreach(hullBreachTutorialSpot);
    }
}
