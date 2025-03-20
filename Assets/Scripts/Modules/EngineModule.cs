using System.Collections;
using UnityEngine;
using UnityEngine.Timeline;

public class EngineModule : BaseModule
{
    protected override void BreakModule()
    {
        // set a public variable flag, which MidGame.cs can read to indicate its broken
        IsBroken = true;
    }

    protected override void FixModule()
    {
        // special code for fixing engine
        //base.FixModule();
         // play the minigame to fix module
        StartCoroutine(PlayFixingMinigame());
    }

    protected override IEnumerator PlayFixingMinigame()
    {
        while(IsBroken)
        {
            // if the engine
            //bool leverPositionsCorrect = false; 

            //Interact with levers to set the correct positions



            yield return null;
        }

        // set a public variable flag, which MidGame.cs can read to indicate its done
        //if(leverPositionsCorerect){FixModule();}
        base.FixModule(); // call the FixModule if the minigame is completed
    
    }
}
