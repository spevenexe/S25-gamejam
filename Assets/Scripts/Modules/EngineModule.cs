using System.Collections;
using UnityEngine;
using UnityEngine.Timeline;

public class EngineModule : BaseModule
{
    protected override void BreakModule()
    {
    }

    protected override void FixModule()
    {
        // special code for fixing engine

        base.FixModule();
    }

    protected override IEnumerator PlayFixingMinigame()
    {
        while(true /* the engine is broken*/)
        {
            // play the game

            yield return null;
        }

        // set a public variable flag, which MidGame.cs can read to indicate its done
    }
}
