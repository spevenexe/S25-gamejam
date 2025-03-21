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
    
    private Color _savedEmissionColor = Color.red;
    private Color _savedLightColor = Color.red;

    protected virtual void Start()
    {
        lights = GetComponentsInChildren<Light>();
        material = GetComponent<MeshRenderer>().material;
        _savedEmissionColor = _defaultEmissionColor;
        _savedLightColor = _defaultLightColor;

        ResetColorDefault();
    }

    public virtual void TurnOff()
    {
        if(lights.Length > 0) _savedLightColor = lights[0].color;
        _savedEmissionColor = material.GetColor("_EmissionColor");
        SetLightColor(Color.black);
        foreach(Light l in lights)
            l.enabled = false;
    }

    private void TurnOn()
    {
        foreach(Light l in lights)
            l.enabled = true;
    }

    public void RestoreColor()
    {
        TurnOn();
        SetLightColor(_savedEmissionColor,_savedLightColor);
    }

    public virtual void SetLightColor(Color emissionColor, Color lightColor)
    {
        TurnOn();
        foreach(Light l in lights)
            l.color = lightColor;
        material.SetColor("_EmissionColor",emissionColor);
    }

    public virtual void SetLightColor(Color color) => SetLightColor(color,color);
    
    public void ResetColorDefault() => SetLightColor(_defaultEmissionColor,_defaultLightColor);
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
            ml.ResetColorDefault();
        }

        if(GUILayout.Button("Turn on",GUILayout.Width(90f)))
        {
            ml.RestoreColor();
        }

        if(GUILayout.Button("Turn off",GUILayout.Width(90f)))
        {
            ml.TurnOff();
        }
    }
}
#endif