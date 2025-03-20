using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class ModuleLights : MonoBehaviour
{
    protected Light[] lights;
    protected Material material;

    // Emission applied to the actual material
    // Light applied to the light components
    [SerializeField] private Color _defaultEmissionColor = new Color(0xFF,0xA3,0x31);
    [SerializeField] private Color _defaultLightColor = new Color(0xB8,0x90,0x35);

    [SerializeField] private Color _alarmEmissionColor = Color.red;
    [SerializeField] private Color _alarmLightColor = Color.red;

    protected virtual void Start()
    {
        lights = GetComponentsInChildren<Light>();
        material = GetComponent<MeshRenderer>().material;
        // SetLightColor(Color.red,Color.red);

        ResetColor();
    }

    public virtual void SetLightColor(Color emissionColor, Color lightColor)
    {
        foreach(Light l in lights)
            l.color = lightColor;
        material.SetColor("_EmissionColor",emissionColor);
    }

    public virtual void SetLightColor(Color color)
    {
        foreach(Light l in lights)
            l.color = color;
        material.SetColor("_EmissionColor",color);
    }
    
    public void ResetColor() => SetLightColor(_defaultEmissionColor,_defaultLightColor);
    public void SetAlarmColor() => SetLightColor(_alarmEmissionColor,_alarmLightColor);
}

#if UNITY_EDITOR
[CustomEditor(typeof(ModuleLights))]
public class ModuleLightsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ModuleLights ml = (ModuleLights) target;
        if(GUILayout.Button("SetAlarm",GUILayout.Width(90f)))
        {
            ml.SetAlarmColor();
        }

        if(GUILayout.Button("Reset",GUILayout.Width(90f)))
        {
            ml.ResetColor();
        }
    }
}
#endif