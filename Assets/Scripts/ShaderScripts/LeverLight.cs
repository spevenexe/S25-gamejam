using UnityEditor;
using UnityEngine;

/// <summary>
/// Light desginated to a lever.
/// <br/>
/// TODO: remove dependency with <c>ModuleLights</c>
/// </summary>
public class LeverLight : ModuleLights
{
    protected override void Start()
    {
        base.Start();
        TurnOff();
    }
}