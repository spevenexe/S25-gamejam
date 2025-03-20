using UnityEditor;
using UnityEngine;

public class LeverLight : ModuleLights
{
    protected override void Start()
    {
        base.Start();
        TurnOff();
    }

    public void TurnOff()
    {
        SetLightColor(Color.black);
        foreach(Light l in lights)
            l.enabled = false;
    }

    public override void SetLightColor(Color emissionColor, Color lightColor)
    {
        foreach(Light l in lights)
            l.enabled = true;
        base.SetLightColor(emissionColor,lightColor);
    }

    public override void SetLightColor(Color color)
    {
        foreach(Light l in lights)
            l.enabled = true;
        base.SetLightColor(color,color);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(LeverLight))]
public class LeverLightEditor : ModuleLightsEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        LeverLight ll = (LeverLight) target;
        if(GUILayout.Button("Turn off",GUILayout.Width(90f)))
        {
            ll.TurnOff();
        }
    }
}
#endif