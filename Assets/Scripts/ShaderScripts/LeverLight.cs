using UnityEditor;
using UnityEngine;

public class LeverLight : ModuleLights
{
    protected override void Start()
    {
        base.Start();
        TurnOff();
    }
}