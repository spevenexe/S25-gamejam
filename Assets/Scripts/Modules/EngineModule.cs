using System.Collections;
using UnityEngine;
using UnityEngine.Timeline;

public class EngineModule : BaseModule
{
    public override void BreakModule(Player player)
    {
        // some setup
        StartCoroutine(PlayFixingMinigame(player));
    }

    public override void FixModule(Player player)
    {
    }

    public override IEnumerator PlayFixingMinigame(Player player)
    {
        while(true /* the engine is broken*/)
        {
            // play the game

            yield return null;
        }

        // set a public variable flag, which MidGame.cs can read to indicate its done
    }
}
