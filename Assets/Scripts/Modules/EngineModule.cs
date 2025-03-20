using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Timeline;

public class EngineModule : BaseModule
{
    private Engine engine;
    [SerializeField] private GameObject leverGroup;
    private Lever [] levers;

    void Awake()
    {
        engine = GetComponentInChildren<Engine>();
        levers = leverGroup.GetComponentsInChildren<Lever>();
    }

    protected override void BreakModule()
    {
        engine.Break();
        foreach(Lever l in levers)
        {
            l.Activate();
        }
        base.BreakModule();
    }

    protected override void FixModule()
    {
        // special code for fixing engine
        engine.StartEngine();
        foreach(Lever l in levers)
        {
            l.Deactivate();
        }
        base.FixModule();
    }

    protected override IEnumerator PlayFixingMinigame()
    {
        Action [] adjustCorrectScore = new Action[levers.Length];
        int score = 0;
        for(int i = 0; i < levers.Length; i++)
        {
            Lever.LeverState correctValue = (Lever.LeverState) UnityEngine.Random.Range(0,2);
            if (levers[i].ActiveState == correctValue) score++;
            adjustCorrectScore[i] = () =>
            {
                score+=(levers[i].ActiveState == correctValue) ? 1 : -1;
            };
            levers[i].AddEvent(adjustCorrectScore[i]);
        }

        while(IsBroken)
        {
            // set a public variable flag, which MidGame.cs can read to indicate its done
            // FixModule() sets IsBroken to false;
            if(score >= levers.Length) FixModule();;
            yield return null;
        }

        for(int i = 0; i < levers.Length; i++)
        {
            levers[i].RemoveEvent(adjustCorrectScore[i]);
        }

    }
}
