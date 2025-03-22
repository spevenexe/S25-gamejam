using System.Collections;
using UnityEngine;

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

    public override void BreakModule()
    {
        engine.Break();
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
        // make sure at least one lever is in the wrong state
        if (levers.Length > 0)
            levers[0].CorrectState =(Lever.LeverState) (((int)levers[0].CorrectState + 1) % 2);

        // generate a solution
        for(int i = 1; i < levers.Length; i++)
        {
            levers[i].CorrectState = (Lever.LeverState) UnityEngine.Random.Range(0,2);
        }

        foreach(Lever l in levers)
        {
            l.Activate();
        }

        while(IsBroken)
        {
            // set a public variable flag, which MidGame.cs can read to indicate its done
            // FixModule() sets IsBroken to false;
            bool correct = true;
            for(int i = 0; i < levers.Length; i++)
            {
                if (levers[i].ActiveState != levers[i].CorrectState) correct = false;
            }
            if (correct) FixModule();
            yield return null; // may want to wait for longer if this causes lag
        }

    }
}
